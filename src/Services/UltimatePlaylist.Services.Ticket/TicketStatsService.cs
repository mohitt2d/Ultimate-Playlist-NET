﻿#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Models.Reward;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Ticket
{
    public class TicketStatsService : ITicketStatsService
    {
        #region Private members

        private readonly Lazy<IRepository<TicketEntity>> TicketRepositoryProvider;

        private readonly Lazy<IRepository<WinningEntity>> WinningRepositoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public TicketStatsService(
            Lazy<IMapper> mapperProvider,
            Lazy<IRepository<TicketEntity>> ticketRepositoryProvider,
            Lazy<IRepository<WinningEntity>> winningRepositoryProvider,
            IOptions<PlaylistConfig> playlistConfig)
        {
            TicketRepositoryProvider = ticketRepositoryProvider;
            WinningRepositoryProvider = winningRepositoryProvider;
            MapperProvider = mapperProvider;
            PlaylistConfig = playlistConfig.Value;
        }

        #endregion

        #region Properties

        private IRepository<TicketEntity> TicketRepository => TicketRepositoryProvider.Value;

        private IRepository<WinningEntity> WinningRepository => WinningRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result<TicketsStatsReadServiceModel>> UserTicketStatsAsync(Guid userExternalId)
        {
            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(PlaylistConfig.TimeZone);
            var now = DateTimeHelper.ToUTCTimeForTimeZoneRelativeTime(DateTime.UtcNow, PlaylistConfig.TimeZone);
            var currentDate = todayDate.Add(PlaylistConfig.StartDateOffSet);
            var nextDate = (now < currentDate) ? currentDate : currentDate.AddDays(1);

            var avaiableTodayTicketsCount = await TicketRepository.CountAsync(new TicketSpecification()
                .WithUser()
                .BySongHistoryUserExternalIdUsingSongRelation(userExternalId)
                .ByTodaysTickets()
                .ByType(TicketType.Daily)
                .OnlyNotUsed());

            avaiableTodayTicketsCount += await TicketRepository.CountAsync(new TicketSpecification()
                .WithUserByPlaylist()
                .ByUserExternalIdUsingPlaylistRelation(userExternalId)
                .ByTodaysTickets()
                .ByType(TicketType.Daily)
                .OnlyNotUsed());

            var avaiableJackpotTicketsCount = await TicketRepository.CountAsync(new TicketSpecification()
                .WithUser()
                .BySongHistoryUserExternalIdUsingSongRelation(userExternalId)
                .ByTodaysTickets()
                .ByType(TicketType.Jackpot)
                .OnlyNotUsed());

            avaiableJackpotTicketsCount += await TicketRepository.CountAsync(new TicketSpecification()
                .WithUserByPlaylist()
                .ByUserExternalIdUsingPlaylistRelation(userExternalId)
                .ByTodaysTickets()
                .ByType(TicketType.Jackpot)
                .OnlyNotUsed());

            var notClaimedWinnings = await WinningRepository.ListAsync(new WinningSpecification().Pagination(new Pagination())
              .ByUserExternalId(userExternalId)
              .WithGame()
              .ByNotCollectedStatus());

            var claimedWinnings = await WinningRepository.ListAsync(new WinningSpecification().Pagination(new Pagination())
              .ByUserExternalId(userExternalId)
              .WithGame()
              .ByStatus(WinningStatus.Paid));

            var ticketsStatsReadServiceModel = new TicketsStatsReadServiceModel()
            {
                TicketsAmountForTodayDrawing = avaiableTodayTicketsCount,
                TicketsAmountForJackpotDrawing = avaiableJackpotTicketsCount,
                PlaylistExpirationTimeStamp = nextDate,
                NextDrawingTimeStamp = nextDate,
                Rewards = Mapper.Map<List<ActiveDrawingRewardReadServiceModel>>(notClaimedWinnings),
                CollectedRewards = Mapper.Map<List<CollectedDrawingRewardReadServiceModel>>(claimedWinnings),
            };

            return ticketsStatsReadServiceModel;
        }
        //new2022-10-14-from
        public async Task<Result<int?>> ReverseTicketStatus(Guid ExternalId, int isErrorTriggered)
        {
            return await GetTicket(ExternalId)
                .Tap(ticket => ChangeErrorTriggered(ticket, isErrorTriggered))
                .Tap(async ticket => await TicketRepository.UpdateAndSaveAsync(ticket))
                .Map(ticket => ticket.IsErrorTriggered);
        }

        public async Task<Result<int>> ReverseTicketsStatus(long ExternalId, int isErrorTriggered)
        {
           return await GetTickets(ExternalId)
                 .Tap(tickets => ChangeErrorsTriggered(tickets, isErrorTriggered))
                 .Tap(async tickets => await TicketRepository.UpdateAndSaveRangeAsync(tickets))
                 .Map(tickets => tickets.Count);
        }

        public async Task<Result<int?>> UserTicketStatus(long userPlaylistSongId)
        {
            return await GetTicketStatus(userPlaylistSongId)
                .Map(ticket => ticket.IsErrorTriggered);
        }

        private async Task<Result<TicketEntity>> GetTicketStatus(long userPlaylistSongId)
        {
            var ticket = await TicketRepository.FirstOrDefaultAsync(new TicketSpecification()
                .ByUserPlaylistSongId(userPlaylistSongId));

            return Result.SuccessIf(ticket != null, ErrorType.CannotFindTicket.ToString())
                .Map(() => ticket);
        }

        private async Task<Result<TicketEntity>> GetTicket(Guid ExternalId)
        {
            var ticket = await TicketRepository.FirstOrDefaultAsync(new TicketSpecification()
                .ByExternalId(ExternalId));

            return Result.SuccessIf(ticket != null, ErrorType.CannotFindTicket.ToString())
                .Map(() => ticket);
        }

        private async Task<Result<IReadOnlyList<TicketEntity>>> GetTickets(long ExternalId)
        {
            var tickets = await TicketRepository.ListAsync(new TicketSpecification()
                .ByUserPlaylistSongId(ExternalId));

            return Result.SuccessIf(tickets != null, ErrorType.CannotFindTicket.ToString())
                .Map(() => tickets);
        }

        private void ChangeErrorTriggered(TicketEntity ticket, int isErrorTriggered)
        {
            ticket.IsErrorTriggered = isErrorTriggered;
        }

        private void ChangeErrorsTriggered(IReadOnlyList<TicketEntity> tickets, int isErrorTriggered)
        {
            for(int i = 0; i < tickets.Count; i++)
            {
                tickets[i].IsErrorTriggered = isErrorTriggered;
            }
        }
        //new2022-10-14-to
        #endregion
    }
}
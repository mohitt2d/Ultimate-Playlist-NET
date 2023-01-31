#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Games.Models.Raffle;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;

#endregion

namespace UltimatePlaylist.Services.Ticket
{
    public class DailyCashTicketsService : IDailyCashTicketsService
    {
        #region Private members

        private readonly Lazy<IRepository<TicketEntity>> TicketRepositoryProvider;

        private readonly Lazy<IMapper> MapperProvider;

        #endregion

        #region Constructor(s)

        public DailyCashTicketsService(
            Lazy<IRepository<TicketEntity>> ticketRepositoryProvider,
            Lazy<IMapper> mapperProvider)
        {
            TicketRepositoryProvider = ticketRepositoryProvider;
            MapperProvider = mapperProvider;
        }

        #endregion

        #region Properties

        private IRepository<TicketEntity> TicketRepository => TicketRepositoryProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public methods

        public async Task<Result<List<RaffleUserTicketReadServiceModel>>> GetTicketsForDailyCashAsync()
        {
            var tickets = new List<RaffleUserTicketReadServiceModel>();

            return await Result.Success()
                .Map(async () => await TicketRepository.ListAsync(new TicketSpecification()
                     .ByType(UltimatePlaylist.Common.Enums.TicketType.Daily)
                     .ByEarnedForPlaylistType()
                     .WithUserByPlaylist()
                     .OnlyNotUsed()))
                .Tap(ticketsForPlaylist => tickets.AddRange(Mapper.Map<List<RaffleUserTicketReadServiceModel>>(ticketsForPlaylist)))
                .Map(async ticketsForPlaylist => await TicketRepository.ListAsync(
                     new TicketSpecification()
                         .ByType(UltimatePlaylist.Common.Enums.TicketType.Daily)
                         .ByEarnedForSongType()
                         .WithUser()
                         .OnlyNotUsed()))
                .Tap(ticketsForSongs => tickets.AddRange(Mapper.Map<List<RaffleUserTicketReadServiceModel>>(ticketsForSongs)))
                .Map(_ => tickets);

        }

        public async Task UseTickets(IEnumerable<Guid> ticketsExternalIds)
        {
            var tickets = await TicketRepository.ListAsync(new TicketSpecification()
                .ByExternalIds(ticketsExternalIds.ToArray()));
            tickets.ToList().ForEach(ticket => ticket.IsUsed = true);
            await TicketRepository.UpdateAndSaveRangeAsync(tickets);
        }

        #endregion
    }
}

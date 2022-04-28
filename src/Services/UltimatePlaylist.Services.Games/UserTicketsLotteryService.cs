#region Usings

using System.Security.Cryptography;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Games.Const;
using UltimatePlaylist.Services.Common.Interfaces.Games;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public class UserTicketsLotteryService : IUserTicketsLotteryService
    {
        #region Private members

        private readonly Lazy<IRepository<UserLotteryEntryEntity>> UserLotteryEntryRepositoryProvider;

        private readonly Lazy<IRepository<TicketEntity>> TicketRepositoryProvider;

        private readonly Lazy<IUltimateWinningClaimService> UltimateWinningClaimServiceProvider;

        #endregion

        #region Constructor(s)

        public UserTicketsLotteryService(
            Lazy<IRepository<TicketEntity>> ticketRepositoryProvider,
            Lazy<IRepository<UserLotteryEntryEntity>> userLotteryEntryRepositoryProvider,
            Lazy<IUltimateWinningClaimService> ultimateWinningClaimServiceProvider)
        {
            TicketRepositoryProvider = ticketRepositoryProvider;
            UserLotteryEntryRepositoryProvider = userLotteryEntryRepositoryProvider;
            UltimateWinningClaimServiceProvider = ultimateWinningClaimServiceProvider;
        }

        #endregion

        #region Properties

        private IRepository<UserLotteryEntryEntity> UserLotteryEntryRepository => UserLotteryEntryRepositoryProvider.Value;

        private IRepository<TicketEntity> TicketRepository => TicketRepositoryProvider.Value;

        private IUltimateWinningClaimService UltimateWinningClaimService => UltimateWinningClaimServiceProvider.Value;

        #endregion

        #region Public methods

        public async Task AddAllUnusedLotteryTickets(long gameId)
        {
            var unusedTickets = await TicketRepository.ListAsync(
                new TicketSpecification()
                .OnlyNotUsed()
                .ByType(TicketType.Jackpot)
                .WithUserByPlaylist());

            var ticketsToUpdate = new List<TicketEntity>();
            var lotteryEntriesToAdd = new List<UserLotteryEntryEntity>();

            foreach (var ticket in unusedTickets)
            {
                lotteryEntriesToAdd.Add(new UserLotteryEntryEntity()
                {
                    FirstNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                    SecondNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                    ThirdNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                    FourthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                    FifthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                    SixthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.SixthNumberRangeExclusive),
                    GameId = gameId,
                    UserId = ticket.UserPlaylistSong.UserPlaylist.User.Id,
                });

                ticket.IsUsed = true;
                ticketsToUpdate.Add(ticket);
            }

            await UserLotteryEntryRepository.AddRangeAsync(lotteryEntriesToAdd);
            await TicketRepository.UpdateAndSaveRangeAsync(ticketsToUpdate);
        }

        public async Task RemoveClaimInfoAsync(long gameId)
        {
            var lotteryEntries = await UserLotteryEntryRepository.ListAsync(
                new UserLotteryEntryEntitySpecification()
                .ByGameId(gameId)
                .WithUser());

            if (lotteryEntries is not null)
            {
                await UltimateWinningClaimService.RemoveArray(lotteryEntries.Select(l => l.User.ExternalId));
            }
        }

        #endregion
    }
}

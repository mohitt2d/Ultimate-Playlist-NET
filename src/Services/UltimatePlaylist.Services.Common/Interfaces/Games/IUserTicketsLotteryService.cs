namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IUserTicketsLotteryService
    {
        public Task AddAllUnusedLotteryTickets(long gameId);

        public Task RemoveClaimInfoAsync(long gameId);
    }
}

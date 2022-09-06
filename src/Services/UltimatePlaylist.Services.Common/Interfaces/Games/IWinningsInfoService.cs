#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IWinningsInfoService
    {
        public Task<Result<WinnersReadServiceModel>> GetWinnersListAsync(Guid userExternalId);
        public Task<Result<List<DailyCashWinnerResponseModel>>> GetDailyWinnersAsync(int pageSize = 10, int pageNumber = 1);
        public Task<Result<List<JackpotWinnersAndNumbersResponseModel>>> GetUltimatePayoutInfoPublicAsync(int pageSize = 10, int pageNumber = 1);

        public Task<Result<List<WinningHistoryReadServicModel>>> GetWinningHistory(Guid userExternalId);
        public Task<Result<List<WinningHistoryReadServicModel>>> GetTodayWinning(Guid userExternalId);
        public Task<Result<List<WinningHistoryReadServicModel>>> GetPastWinnings(Guid userExternalId);

    }
}

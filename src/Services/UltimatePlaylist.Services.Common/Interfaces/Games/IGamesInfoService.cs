#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IGamesInfoService
    {
        public Task<Result<GamesinfoReadServiceModel>> GetGamesInfoAsync(Guid userExternalId);

        public Task<Result<GamesinfoReadServiceModel>> CheckNewestGame(Guid userExternalId);

        public Task<Result<GamesinfoReadServiceModel>> ClaimWinningsAsync(Guid userExternalId);
    }
}

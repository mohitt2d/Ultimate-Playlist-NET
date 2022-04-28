#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IWinningsInfoService
    {
        public Task<Result<WinnersReadServiceModel>> GetWinnersListAsync(Guid userExternalId);
    }
}

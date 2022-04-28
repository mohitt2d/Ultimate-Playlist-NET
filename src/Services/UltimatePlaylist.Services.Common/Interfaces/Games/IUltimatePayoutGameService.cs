#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IUltimatePayoutGameService
    {
        Task<Result<UltimatePayoutReadServiceModel>> PostUltimatePayoutInfoAsync(Guid userExternalId, List<int> ultimatePayoutNumbers);

        Task<Result<UltimatePayoutReadServiceModel>> GetUltimatePayoutInfoAsync(Guid userExternalId);

        Task<Result<UltimatePayoutReadServiceModel>> ClaimWinningsAsync(Guid userExternalId);

        Task<Result<UltimatePayoutReadServiceModel>> CheckNewestGame(Guid userExternalId);
    }
}

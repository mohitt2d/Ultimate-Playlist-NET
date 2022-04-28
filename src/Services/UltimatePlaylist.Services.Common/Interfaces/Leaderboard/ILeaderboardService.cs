#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Leaderboard;

#endregion
namespace UltimatePlaylist.Services.Common.Interfaces.Leaderboard
{
    public interface ILeaderboardService
    {
        Task<Result<LeaderboardReadServiceModel>> GetLeaderboardInfoAsync(Guid userExternalId);
    }
}

#region Usings

using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Games
{
    public interface IGamesWinningCollectionService
    {
        Task<GamesCollectionReadServiceModel> Get(Guid userExternalId, CancellationToken cancellationToken = default);

        Task Set(Guid userExternalId, CancellationToken cancellationToken = default);

        Task Remove(Guid userExternalId, CancellationToken cancellationToken = default);

        Task RemoveArray(IEnumerable<Guid> userExternalIds, CancellationToken cancellationToken = default);
    }
}

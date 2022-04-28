#region Usings

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Base;
using UltimatePlaylist.Services.Common.Interfaces.Games;
using UltimatePlaylist.Services.Common.Models.Games;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public class GamesWinningCollectionService : BaseCacheService, IGamesWinningCollectionService
    {
        #region Private Members

        private const string TicketsKey = "gamescollection";

        #endregion

        #region Constructor

        public GamesWinningCollectionService(Lazy<IDistributedCache> cacheProvider, IOptions<PlaylistConfig> config)
            : base(cacheProvider, TicketsKey, config)
        {
        }

        #endregion

        #region Public Methods

        public async Task<GamesCollectionReadServiceModel> Get(Guid userExternalId, CancellationToken cancellationToken = default) => await Get<GamesCollectionReadServiceModel>(userExternalId.ToString(), cancellationToken);

        public async Task Set(Guid userExternalId, CancellationToken cancellationToken = default) => await Set(userExternalId.ToString(), new GamesCollectionReadServiceModel() { Collected = true, }, cancellationToken);

        public async Task Remove(Guid userExternalId, CancellationToken cancellationToken = default) => await Remove(userExternalId.ToString(), cancellationToken);

        public async Task RemoveArray(IEnumerable<Guid> userExternalIds, CancellationToken cancellationToken = default)
        {
            foreach (var item in userExternalIds)
            {
                await Remove(item.ToString(), cancellationToken);
            }
        }

        #endregion
    }
}

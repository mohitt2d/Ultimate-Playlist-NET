#region Usings

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Base;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Song
{
    public class SongAntibotService : BaseCacheService, ISongAntibotService
    {
        #region Private Members

        private const string AntibotKey = "playlistactions";
        private readonly PlaylistConfig Config;

        #endregion

        #region Constructor

        public SongAntibotService(Lazy<IDistributedCache> cacheProvider, IOptions<PlaylistConfig> playlistConfigOptions, IOptions<PlaylistConfig> config)
            : base(cacheProvider, AntibotKey, config)
        {
            Config = playlistConfigOptions.Value;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ShouldActivateAsync(Guid userExternalId, CancellationToken cancellationToken = default)
        {
            var counter = await Get<NoActionCountReadServiceModel>(userExternalId.ToString(), cancellationToken);

            if (counter?.NoActionCounter != null && counter.NoActionCounter >= Config.AntibotSongsCount)
            {
                await ResetCounterAsync(userExternalId, cancellationToken);
                return true;
            }

            return false;
        }

        public async Task AddNoActionAsync(Guid userExternalId, CancellationToken cancellationToken = default)
        {
            var counter = await Get<NoActionCountReadServiceModel>(userExternalId.ToString(), cancellationToken);

            await Set(
                userExternalId.ToString(),
                new NoActionCountReadServiceModel()
                {
                    NoActionCounter = counter?.NoActionCounter is not null ? ++counter.NoActionCounter : 1,
                }, cancellationToken);
        }

        public async Task ResetCounterAsync(Guid userExternalId, CancellationToken cancellationToken = default) => await Remove(userExternalId.ToString(), cancellationToken);

        #endregion
    }
}

#region Usings

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Converters;
using UltimatePlaylist.Services.Common.Base;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Models.Playlist;

#endregion

namespace UltimatePlaylist.Services.Playlist
{
    public class UserPlaylistService : BaseCacheService, IUserPlaylistService
    {
        #region Consts

        private const string UserPlaylistKey = "userplayliststore";

        #endregion

        #region Constructor

        public UserPlaylistService(Lazy<IDistributedCache> cacheServiceProvider, IOptions<PlaylistConfig> config)
            : base(cacheServiceProvider, UserPlaylistKey, config)
        {
        }

        #endregion

        #region Public Methods

        public async Task Set(Guid userExternalId, PlaylistReadServiceModel playlistReadServiceModel) => await Set(userExternalId.ToString(), playlistReadServiceModel);

        public async Task<PlaylistReadServiceModel> Get(Guid userExternalId) => await Get<PlaylistReadServiceModel>(userExternalId.ToString());

        #endregion
    }
}

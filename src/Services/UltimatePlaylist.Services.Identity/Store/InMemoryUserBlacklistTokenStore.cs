#region Usings

using Microsoft.Extensions.Caching.Memory;
using UltimatePlaylist.Common.Mvc.Interface;

#endregion

namespace UltimatePlaylist.Services.Identity.Store
{
    public class InMemoryUserBlacklistTokenStore : IUserBlacklistTokenStore
    {
        #region Const

        private const string MemoryCachePrefix = "UserToken";

        #endregion

        #region Private Members

        private readonly Lazy<IMemoryCache> MemoryCacheProvider;

        #endregion

        #region Constructor

        public InMemoryUserBlacklistTokenStore(
            Lazy<IMemoryCache> memoryCacheProvider)
        {
            MemoryCacheProvider = memoryCacheProvider;
        }

        #endregion

        #region Properties

        private IMemoryCache MemoryCache => MemoryCacheProvider.Value;

        #endregion

        #region Public Methods

        public void Set(Guid externalId, string token)
        {
            var key = BuildKey(externalId);
            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1),
            };

            MemoryCache.Set(key, token, options);
        }

        public bool TryGet(Guid externalId, out string token)
        {
            var key = BuildKey(externalId);

            return MemoryCache.TryGetValue(key, out token);
        }

        #endregion

        #region Private Methods

        private string BuildKey(Guid externalId) => $"{MemoryCachePrefix}_{externalId}";

        #endregion
    }
}

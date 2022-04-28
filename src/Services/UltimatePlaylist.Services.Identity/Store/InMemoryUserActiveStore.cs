#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using UltimatePlaylist.Services.Common.Interfaces.Identity;

#endregion

namespace UltimatePlaylist.Services.Identity.Store
{
    public class InMemoryUserActiveStore : IUserActiveStore
    {
        #region Const

        private const string MemoryCachePrefix = "UserActve";

        #endregion

        #region Private Members

        private readonly Lazy<IMemoryCache> MemoryCacheProvider;

        #endregion

        #region Constructor

        public InMemoryUserActiveStore(
            Lazy<IMemoryCache> memoryCacheProvider)
        {
            MemoryCacheProvider = memoryCacheProvider;
        }

        #endregion

        #region Properties

        private IMemoryCache MemoryCache => MemoryCacheProvider.Value;

        #endregion

        #region Public Methods

        public void Set(Guid externalId, bool isActive)
        {
            var key = BuildKey(externalId);
            var options = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1),
            };

            MemoryCache.Set(key, isActive, options);
        }

        public bool TryGet(Guid externalId, out bool isActive)
        {
            var key = BuildKey(externalId);

            return MemoryCache.TryGetValue(key, out isActive);
        }

        #endregion

        #region Private Methods

        private string BuildKey(Guid externalId) => $"{MemoryCachePrefix}_{externalId}";

        #endregion
    }
}

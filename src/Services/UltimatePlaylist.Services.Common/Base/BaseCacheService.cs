#region Usings

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Mvc.Converters;
using UltimatePlaylist.Common.Mvc.Helpers;

#endregion

namespace UltimatePlaylist.Services.Common.Base
{
    public abstract class BaseCacheService
    {
        #region Private Members

        private readonly Lazy<IDistributedCache> CacheProvider;
        private readonly JsonSerializerOptions SerializerOptions;
        private readonly DistributedCacheEntryOptions CacheOptions;
        private readonly string TypeKey;

        #endregion

        #region Constructor

        public BaseCacheService(Lazy<IDistributedCache> cacheProvider, string typeKey, IOptions<PlaylistConfig> config)
        {
            CacheProvider = cacheProvider;
            TypeKey = typeKey;
            SerializerOptions = new JsonSerializerOptions();
            SerializerOptions.Converters.Add(new TicksTimeSpanConverter());
            var gameOptions = config.Value;

            var todayDate = DateTimeHelper.ToTodayUTCTimeForTimeZoneRelativeTime(gameOptions.TimeZone);
            var currentDate = todayDate.Add(gameOptions.StartDateOffSet);
            var nextDate = currentDate.AddDays(1);

            CacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = nextDate,
            };
        }

        #endregion

        #region Private properiteis

        private IDistributedCache CacheService => CacheProvider.Value;

        #endregion

        #region Protected Methods

        protected async Task<T> Get<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await CacheService.GetAsync(GetKey(key), cancellationToken);
            if (data is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data, SerializerOptions);
        }

        protected async Task Set<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            var serializedData = JsonSerializer.SerializeToUtf8Bytes(data, SerializerOptions);
            await CacheService.SetAsync(GetKey(key), serializedData, CacheOptions, cancellationToken);
        }

        protected async Task Remove(string key, CancellationToken cancellationToken = default)
        {
            await CacheService.RemoveAsync(GetKey(key), cancellationToken);
        }

        protected string GetKey(string key) => $"{TypeKey}_{key}";

        #endregion
    }
}

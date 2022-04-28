#region Usings

using System;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;
using UltimatePlaylist.Services.Common.Models.Webhook;

#endregion

namespace UltimatePlaylist.Services.Media
{
    public class MediaServicesEventGridConfigProviderService : IEventGridConfigProviderService
    {
        #region Private fields

        private readonly Lazy<IOptions<AzureMediaServicesConfig>> AzureMediaServicesConfigProvider;

        #endregion

        #region Constructor(s)

        public MediaServicesEventGridConfigProviderService(
            Lazy<IOptions<AzureMediaServicesConfig>> azureMediaServicesConfigProvider)
        {
            AzureMediaServicesConfigProvider = azureMediaServicesConfigProvider;
        }

        #endregion

        #region Properties

        private AzureMediaServicesConfig AzureMediaServicesConfig => AzureMediaServicesConfigProvider.Value.Value;

        #endregion

        #region Public methods

        public EventGridConfigReadServiceModel GetConfig()
        {
            return new EventGridConfigReadServiceModel
            {
                QueryKey = AzureMediaServicesConfig.EventGridQueryKey,
                SubscriptionName = AzureMediaServicesConfig.EventGridSubscriptionName,
            };
        }

        #endregion
    }
}

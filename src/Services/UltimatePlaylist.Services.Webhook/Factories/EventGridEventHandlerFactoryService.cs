#region Usings

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;

#endregion

namespace UltimatePlaylist.Services.Webhook.Factories
{
    public class EventGridEventHandlerFactoryService : IEventGridEventHandlerFactoryService
    {
        #region Private fields

        private readonly Lazy<IServiceProvider> ServiceProviderLazy;

        #endregion

        #region Constructor(s)

        public EventGridEventHandlerFactoryService(Lazy<IServiceProvider> serviceProviderLazy)
        {
            ServiceProviderLazy = serviceProviderLazy;
        }

        #endregion

        #region Properties

        private IServiceProvider ServiceProvider => ServiceProviderLazy.Value;

        #endregion

        #region Public methods

        public IEventGridEventHandler CreateHandler(EventGridEventType eventGridEventType)
        {
            var handler = ServiceProvider
                .GetServices<IEventGridEventHandler>()
                .FirstOrDefault(h => h.EventGridEventType == eventGridEventType);
            if (handler is null)
            {
                throw new InvalidOperationException(
                    $"No Event Grid event handler found for {eventGridEventType} event type.");
            }

            return handler;
        }

        #endregion
    }
}

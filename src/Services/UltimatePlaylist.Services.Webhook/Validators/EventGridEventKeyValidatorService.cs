#region Usings

using System;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;

#endregion

namespace UltimatePlaylist.Services.Webhook.Validators
{
    public class EventGridEventKeyValidatorService : IEventGridEventKeyValidatorService
    {
        #region Private fields

        private readonly Lazy<IEventGridConfigProviderService> EventGridQueryKeyProviderServiceProvider;

        #endregion

        #region Constructor(s)

        public EventGridEventKeyValidatorService(
            Lazy<IEventGridConfigProviderService> eventGridQueryKeyProviderServiceProvider)
        {
            EventGridQueryKeyProviderServiceProvider = eventGridQueryKeyProviderServiceProvider;
        }

        #endregion

        #region Properties

        private IEventGridConfigProviderService EventGridQueryKeyProvider => EventGridQueryKeyProviderServiceProvider.Value;

        #endregion

        #region Public methods

        public Result Validate(string queryKey)
        {
            var queryKeyToCompare = EventGridQueryKeyProvider.GetConfig().QueryKey;

            return Result.SuccessIf(
                queryKeyToCompare.Equals(queryKey),
                ErrorType.InvalidKey.ToString());
        }

        #endregion
    }
}

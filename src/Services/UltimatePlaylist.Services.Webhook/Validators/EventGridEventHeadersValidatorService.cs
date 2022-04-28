#region Usings

using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Primitives;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Interfaces.Webhook;

#endregion

namespace UltimatePlaylist.Services.Webhook.Validators
{
    public class EventGridEventHeadersValidatorService : IEventGridEventHeadersValidatorService
    {
        #region Private consts

        private const string AzureEventGridSubscriptionNameHeaderName = "aeg-subscription-name";

        #endregion

        #region Private fields

        private readonly Lazy<IEventGridConfigProviderService> EventGridConfigProviderServiceProvider;

        #endregion

        #region Constructor(s)

        public EventGridEventHeadersValidatorService(
            Lazy<IEventGridConfigProviderService> eventGridConfigProviderServiceProvider)
        {
            EventGridConfigProviderServiceProvider = eventGridConfigProviderServiceProvider;
        }

        #endregion

        #region Propeties

        private IEventGridConfigProviderService EventGridConfigProvider => EventGridConfigProviderServiceProvider.Value;

        #endregion

        #region Public methods

        public Result Validate(IDictionary<string, StringValues> headers)
        {
            var subscriptionNameToCompare = EventGridConfigProvider.GetConfig().SubscriptionName;

            return GetEventGridSubscriptionName(headers)
                .Bind(subscriptionName => ValidateSubscriptionName(subscriptionName, subscriptionNameToCompare));
        }

        #endregion

        #region Private methods

        private Result<string> GetEventGridSubscriptionName(IDictionary<string, StringValues> headers)
        {
            if (!headers.TryGetValue(AzureEventGridSubscriptionNameHeaderName, out var stringValues))
            {
                return Result.Failure<string>(ErrorType.MissingEventGridSubscriptionName.ToString());
            }

            return stringValues
                .TryFirst()
                .ToResult(ErrorType.MissingEventGridSubscriptionName.ToString());
        }

        private Result ValidateSubscriptionName(string subscriptionName, string subscriptionNameToCompare)
        {
            return Result.SuccessIf(
                subscriptionName.Equals(subscriptionNameToCompare, StringComparison.InvariantCultureIgnoreCase),
                ErrorType.InvalidEventGridSubscriptionName.ToString());
        }

        #endregion
    }
}
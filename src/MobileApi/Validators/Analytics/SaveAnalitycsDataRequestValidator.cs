#region Usings

using System;
using System.Collections.Generic;
using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Analytics;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Analytics
{
    public class SaveAnalitycsDataRequestValidator : AbstractValidator<SaveAnalyticsDataRequestModel>
    {
        #region Public Methods

        public SaveAnalitycsDataRequestValidator()
        {
            RuleFor(p => p.Params)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.EventType)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MustValueInEnum<SaveAnalyticsDataRequestModel, AnalitycsEventType>();

            RuleFor(p => GetValueFromDictionary(p.Params, "SongExternalId"))
                .MustBeGuid()
                .WithName("SongExternalId");

            RuleFor(p => GetValueFromDictionary(p.Params, "PlaylistExternalId"))
                .MustBeGuid()
                .WithName("PlaylistExternalId");

            When(p => CheckIfActualListeningSecondRequired(p.EventType), () =>
            {
                RuleFor(p => GetValueFromDictionary(p.Params, "ActualListeningSecond"))
                 .NotEmpty()
                 .WithMessage(ValidationMessages.CannotBeEmpty)
                 .MustBeDigits()
                 .WithName("ActualListeningSecond");
            });
        }

        #endregion

        #region Private Methods

        public bool CheckIfActualListeningSecondRequired(string eventType)
        {
            Enum.TryParse(eventType, out AnalitycsEventType enumValue);
            return CheckIfActualListeningSecondIsRequiredForEventType(enumValue);
        }

        private static bool CheckIfActualListeningSecondIsRequiredForEventType(AnalitycsEventType analitycsEventType) =>
           analitycsEventType switch
           {
               AnalitycsEventType.StartSong => false,
               AnalitycsEventType.ThirtySecondsOfSong => false,
               AnalitycsEventType.SixtySecondsOfSong => false,
               AnalitycsEventType.EntireSong => false,
               AnalitycsEventType.BreakListening => true,
               AnalitycsEventType.SkipSong => true,
               AnalitycsEventType.ThreeSongsWithoutSkip => false,
               AnalitycsEventType.ExpirationOfSkipRefresh => false,
               _ => throw new Exception(ErrorType.NotSupportedAnalyticsEventType.ToString()),
           };

        private string GetValueFromDictionary(Dictionary<string, string> dictionary, string valueToFind)
        {
            if (dictionary != null && dictionary.TryGetValue(valueToFind, out string result))
            {
                return result;
            }

            return string.Empty;
        }

        #endregion

    }
}

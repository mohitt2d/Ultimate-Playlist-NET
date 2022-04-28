#region Usings

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class CustomFluentValidator
    {
        public const int IsoE164MaximumLength = 20;
        public const int IsoE164MinimumLength = 10;

        #region Custom

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Length(IsoE164MinimumLength, IsoE164MaximumLength)
                .WithMessage(string.Format(ValidationMessages.InvalidLength, IsoE164MinimumLength, IsoE164MaximumLength))
                .Must(phoneNumber => phoneNumber?.All(sign => char.IsDigit(sign)) ?? true)
                .WithMessage(ValidationMessages.MustBeNumber);
        }

        public static IRuleBuilderOptions<T, string> EmailAddressWithExtension<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            Regex rgx = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return ruleBuilder
                .Must(email => !string.IsNullOrEmpty(email) && rgx.IsMatch(email))
                .WithMessage(ValidationMessages.MustBeEmail);
        }

        public static IRuleBuilderOptions<T, TimeSpan> SongDurationMinumum<T>(this IRuleBuilder<T, TimeSpan> ruleBuilder)
        {
            return ruleBuilder
                .Must(duration => duration.TotalSeconds >= SongConst.DurationMinumumLength)
                .WithMessage(string.Format(ValidationMessages.MinimumSongDuration, SongConst.DurationMinumumLength));
        }

        #endregion

        #region Logical

        public static IRuleBuilderOptions<T, string> MustBeDigits<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => value?.All(char.IsDigit) ?? true)
                .WithError(ValidationErrorType.OnlyDigitsAllowed);
        }

        public static IRuleBuilderOptions<T, string> MustBeSpotifyDeepLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => value.StartsWith(DspConst.Spotify))
                .WithError(ValidationErrorType.IncorrectSpotifyDeeplink);
        }

        public static IRuleBuilderOptions<T, string> MustBeAppleMusicDeepLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => value.StartsWith(DspConst.AppleMusic))
                .WithError(ValidationErrorType.IncorrectAppleMusicDeeplink);
        }

        public static IRuleBuilderOptions<T, string> MustBeInstagramLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.StartsWith(SocialMediaConst.Instagram))
                .WithError(ValidationErrorType.IncorrectInstagramLink);
        }

        public static IRuleBuilderOptions<T, string> MustBeFacebookLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.StartsWith(SocialMediaConst.Facebook))
                .WithError(ValidationErrorType.IncorrectFacebookLink);
        }

        public static IRuleBuilderOptions<T, string> MustBeYoutubeLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.StartsWith(SocialMediaConst.Youtube))
                .WithError(ValidationErrorType.IncorrectYoutubeLink);
        }

        public static IRuleBuilderOptions<T, string> MustBeSnapchatLink<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.StartsWith(SocialMediaConst.Snapchat))
                .WithError(ValidationErrorType.IncorrectSnapchatLink);
        }

        public static IRuleBuilderOptions<T, int?> MustBeSongRatingDigits<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder.Must(rating => rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
                .WithError(ValidationErrorType.OnlyDigitsBetweenOneAndFiveAllowed);
        }

        public static IRuleBuilderOptions<T, string> MustValueInEnum<T, TEnum>(this IRuleBuilder<T, string> ruleBuilder)
            where TEnum : struct
        {
            return ruleBuilder.Must(value => Enum.TryParse(value, out TEnum enumValue))
                .WithError(ValidationErrorType.IncorrectTypeValue);
        }

        public static IRuleBuilderOptions<T, string> MustBeGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(guid => !string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out var parsedGuid))
                .WithError(ValidationErrorType.IncorrectGuidValue);
        }

        public static IRuleBuilderOptions<T, string> MustDateTimeStamp<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(dateString => string.IsNullOrEmpty(dateString) || dateString.TryFromStringUnixTimestamp(out var date))
                .WithError(ValidationErrorType.InvalidTimeStampValue);
        }

        #endregion

        #region Error

        public static IRuleBuilderOptions<T, string> WithError<T>(this IRuleBuilderOptions<T, string> ruleBuilder, ValidationErrorType validationErrorType)
        {
            return ruleBuilder
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(validationErrorType.ToString());
        }

        public static IRuleBuilderOptions<T, bool?> WithError<T>(this IRuleBuilderOptions<T, bool?> ruleBuilder, ValidationErrorType validationErrorType)
        {
            return ruleBuilder
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(validationErrorType.ToString());
        }

        public static IRuleBuilderOptions<T, DateTime?> WithError<T>(this IRuleBuilderOptions<T, DateTime?> ruleBuilder, ValidationErrorType validationErrorType)
        {
            return ruleBuilder
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(validationErrorType.ToString());
        }

        public static IRuleBuilderOptions<T, Guid?> WithError<T>(this IRuleBuilderOptions<T, Guid?> ruleBuilder, ValidationErrorType validationErrorType)
        {
            return ruleBuilder
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(validationErrorType.ToString());
        }

        public static IRuleBuilderOptions<T, int?> WithError<T>(this IRuleBuilderOptions<T, int?> ruleBuilder, ValidationErrorType validationErrorType)
        {
            return ruleBuilder
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .WithMessage(validationErrorType.ToString());
        }

        #endregion

        #region Global Error

        #endregion

    }
}

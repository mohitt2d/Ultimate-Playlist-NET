﻿#region Usings

using System;
using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.Common.Mvc.Helpers;
using UltimatePlaylist.MobileApi.Models.UserProfile;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.UserProfile
{
    public class UserProfileEditRequestValidator : AbstractValidator<UserProfileEditRequestModel>
    {
        public UserProfileEditRequestValidator()
        {
            RuleFor(p => p.Username)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => x is not null && !x.Contains(' '))
                .WithMessage(ValidationMessages.UsernameCannotContainsSpace);

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => x is not null && !x.Contains(' '))
                .WithMessage(ValidationMessages.FirstNameCannotContainsSpace);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => x is not null && !x.Contains(' '))
                .WithMessage(ValidationMessages.LastNameCannotContainsSpace);

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .EmailAddress()
                .WithMessage(ValidationMessages.MustBeEmail)
                .EmailAddressWithExtension();
            
            RuleFor(p => p.BirthDate)
               .NotEqual(DateTime.UnixEpoch)
               .WithMessage(ValidationMessages.CannotBeEmpty)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty)
               .LessThan(DateTime.UtcNow.AddYears(-18))
               .WithMessage(ValidationMessages.YouMustBeOlder);

            RuleFor(p => p.GenderExternalId)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.ZipCode)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty)
               .MustBeDigits()
               .WithMessage(ValidationMessages.MustBeNumber)
               .Length(5)
               .WithMessage(string.Format(ValidationMessages.MustBeLength, 5))
               .MustAsync(async (zipCode, cancellation) =>
               {
                   bool isValid = await GeoCoderHelper.IsUSAZipCodeAsync(zipCode);
                   return isValid;
               })
               .WithMessage(string.Format(ValidationMessages.MustBeUSZipCode));
        }
    }
}
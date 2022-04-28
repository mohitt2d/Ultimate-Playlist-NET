#region Usings

using System;
using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequestModel>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => !string.IsNullOrEmpty(x) && !x.Contains(' '))
                .WithMessage(ValidationMessages.UsernameCannotContainsSpace);

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => !string.IsNullOrEmpty(x) && !x.Contains(' '))
                .WithMessage(ValidationMessages.FirstNameCannotContainsSpace);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .Must(x => !string.IsNullOrEmpty(x) && !x.Contains(' '))
                .WithMessage(ValidationMessages.LastNameCannotContainsSpace);

            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .EmailAddress()
                .WithMessage(ValidationMessages.MustBeEmail)
                .EmailAddressWithExtension();

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MinimumLength(8)
                .WithMessage(string.Format(ValidationMessages.ToShort, 8));

            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .PhoneNumber()
                .WithMessage(ValidationMessages.MustBePhoneNumber);

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
               .WithMessage(string.Format(ValidationMessages.MustBeLength, 5));

            RuleFor(p => p.IsTermsAndConditionsRead)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty)
               .NotEqual(false)
               .WithMessage(ValidationMessages.TermsAndCoditions);
        }
    }
}

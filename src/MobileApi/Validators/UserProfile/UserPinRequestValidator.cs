#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.UserProfile;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.UserProfile
{
    public class UserPinRequestValidator : AbstractValidator<UserPinRequestModel>
    {
        public UserPinRequestValidator()
        {
            RuleFor(p => p.Pin)
                .NotEmpty()
                .WithMessage(ValidationMessages.MustBeValidPin)
                .MustBeDigits()
                .WithMessage(ValidationMessages.MustBeValidPin)
                .Length(4)
                .WithMessage(ValidationMessages.MustBeValidPin);
        }
    }
}
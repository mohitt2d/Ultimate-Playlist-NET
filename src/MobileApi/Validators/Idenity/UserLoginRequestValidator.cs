#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequestModel>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .EmailAddress()
                .WithMessage(ValidationMessages.MustBeEmail)
                .EmailAddressWithExtension();

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

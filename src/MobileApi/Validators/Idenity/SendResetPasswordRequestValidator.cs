#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class SendResetPasswordRequestValidator : AbstractValidator<SendResetPasswordRequestModel>
    {
        public SendResetPasswordRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .EmailAddress()
                .WithMessage(ValidationMessages.MustBeEmail)
                .EmailAddressWithExtension();
        }
    }
}

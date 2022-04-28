#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class SendEmailConfirmationRequestValidator : AbstractValidator<SendEmailConfirmationRequestModel>
    {
        public SendEmailConfirmationRequestValidator()
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

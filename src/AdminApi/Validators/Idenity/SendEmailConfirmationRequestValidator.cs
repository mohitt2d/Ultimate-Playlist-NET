#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Idenity
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

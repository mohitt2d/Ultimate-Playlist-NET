#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class EmailChangedConfirmationRequestValidator : AbstractValidator<EmailChangedConfirmationRequestModel>
    {
        public EmailChangedConfirmationRequestValidator()
        {
            RuleFor(p => p.Token)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

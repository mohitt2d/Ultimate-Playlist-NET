#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;
using UltimatePlaylist.Common.Const;

#endregion
namespace UltimatePlaylist.AdminApi.Validators.Idenity
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

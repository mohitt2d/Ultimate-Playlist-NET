#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(p => p.CurrentPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

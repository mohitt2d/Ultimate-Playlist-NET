#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MinimumLength(10)
                .WithMessage(string.Format(ValidationMessages.ToShort, 10));

            RuleFor(p => p.Token)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

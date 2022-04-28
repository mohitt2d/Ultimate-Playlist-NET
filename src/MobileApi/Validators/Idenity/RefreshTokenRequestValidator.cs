#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Idenity
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestModel>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(p => p.RefreshToken)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

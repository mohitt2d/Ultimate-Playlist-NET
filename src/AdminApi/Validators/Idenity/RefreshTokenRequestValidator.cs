#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Idenity
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequestModel>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(p => p.RefreshToken)
                .NotEmpty();
        }
    }
}

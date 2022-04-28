#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Idenity
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty();
            RuleFor(p => p.Token)
                .NotEmpty();
        }
    }
}

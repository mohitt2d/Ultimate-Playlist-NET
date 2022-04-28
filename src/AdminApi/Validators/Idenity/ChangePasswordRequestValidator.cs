#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Idenity
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(p => p.CurrentPassword)
                .NotEmpty();
            RuleFor(p => p.NewPassword)
                .NotEmpty();
        }
    }
}

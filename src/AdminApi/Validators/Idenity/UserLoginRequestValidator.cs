#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Identity;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Idenity
{
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequestModel>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty();
            RuleFor(p => p.Password)
                .NotEmpty();
        }
    }
}

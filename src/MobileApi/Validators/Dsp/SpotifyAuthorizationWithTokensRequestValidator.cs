#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Dsp.Spotify;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Dsp
{
    public class SpotifyAuthorizationWithTokensRequestValidator : AbstractValidator<SpotifyAuthorizationWithTokensRequestModel>
    {
        public SpotifyAuthorizationWithTokensRequestValidator()
        {
            RuleFor(p => p.AccessToken)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.RefreshToken)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.AccessTokenExpirationDate)
               .NotEmpty()
               .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

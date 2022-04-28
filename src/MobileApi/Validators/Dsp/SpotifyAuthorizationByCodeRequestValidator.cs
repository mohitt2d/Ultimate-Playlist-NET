#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Dsp.Spotify;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Dsp
{
    public class SpotifyAuthorizationByCodeRequestValidator : AbstractValidator<SpotifyAuthorizationByCodeRequestModel>
    {
        public SpotifyAuthorizationByCodeRequestValidator()
        {
            RuleFor(p => p.Code)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

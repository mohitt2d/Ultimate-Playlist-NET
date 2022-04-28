#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Dsp.Spotify;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Dsp
{
    public class AddSongToSpotifyRequestValidator : AbstractValidator<AddSongToSpotifyRequestModel>
    {
        public AddSongToSpotifyRequestValidator()
        {
            RuleFor(p => p.SongExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.PlaylistExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

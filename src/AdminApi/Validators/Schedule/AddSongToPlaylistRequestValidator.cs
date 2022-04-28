#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Schedule
{
    public class AddSongToPlaylistRequestValidator : AbstractValidator<AddSongToPlaylistRequestModel>
    {
        public AddSongToPlaylistRequestValidator()
        {
            RuleFor(p => p.PlaylistExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.SongExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

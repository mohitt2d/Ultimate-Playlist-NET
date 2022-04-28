#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Schedule
{
    public class PlaylistSongsRequestValidator : AbstractValidator<PlaylistSongsRequestModel>
    {
        public PlaylistSongsRequestValidator()
        {
            RuleFor(p => p.PlaylistExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}
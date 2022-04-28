#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Schedule
{
    public class RemoveSongFromPlaylistRequestValidator : AbstractValidator<RemoveSongFromPlaylistRequestModel>
    {
        public RemoveSongFromPlaylistRequestValidator()
        {
            RuleFor(p => p.ExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.SongExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

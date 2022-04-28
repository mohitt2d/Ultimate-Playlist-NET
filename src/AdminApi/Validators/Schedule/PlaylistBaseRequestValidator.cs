#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Schedule;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Schedule
{
    public class PlaylistBaseRequestValidator : AbstractValidator<PlaylistBaseRequestModel>
    {
        public PlaylistBaseRequestValidator()
        {
            RuleFor(p => p.ExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

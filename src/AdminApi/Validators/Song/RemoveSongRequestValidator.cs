#region Usings

using FluentValidation;
using UltimatePlaylist.AdminApi.Models.Song;
using UltimatePlaylist.Common.Const;

#endregion

namespace UltimatePlaylist.AdminApi.Validators.Song
{
    public class RemoveSongRequestValidator : AbstractValidator<RemoveSongRequestModel>
    {
        public RemoveSongRequestValidator()
        {
            RuleFor(p => p.ExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

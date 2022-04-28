#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Song;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Song
{
    public class RateSongRequestValidator : AbstractValidator<RateSongRequestModel>
    {
        public RateSongRequestValidator()
        {
            RuleFor(p => p.ExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.PlaylistExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);

            RuleFor(p => p.Rating)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty)
                .MustBeSongRatingDigits();
        }
    }
}

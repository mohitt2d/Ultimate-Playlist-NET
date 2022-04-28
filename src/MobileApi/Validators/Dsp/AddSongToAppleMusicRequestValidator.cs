#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.MobileApi.Models.Dsp.AppleMusic;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Dsp
{
    public class AddSongToAppleMusicRequestValidator : AbstractValidator<AddSongToAppleMusicRequestModel>
    {
        public AddSongToAppleMusicRequestValidator()
        {
            RuleFor(p => p.SongExternalId)
                .NotEmpty()
                .WithMessage(ValidationMessages.CannotBeEmpty);
        }
    }
}

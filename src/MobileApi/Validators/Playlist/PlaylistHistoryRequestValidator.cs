#region Usings

using FluentValidation;
using UltimatePlaylist.Common.Mvc.Extensions;
using UltimatePlaylist.MobileApi.Models.Playlist;

#endregion

namespace UltimatePlaylist.MobileApi.Validators.Playlist
{
    public class PlaylistHistoryRequestValidator : AbstractValidator<PlaylistHistoryRequestModel>
    {
        public PlaylistHistoryRequestValidator()
        {
            RuleFor(p => p.SelectedDateTimeStamp)
                .MustDateTimeStamp();
        }
    }
}

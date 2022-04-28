#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongRatingService
    {
        Task<Result<EarnedTicketsReadServiceModel>> RateSongAsync(Guid userExternalId, RateSongWriteServiceModel rateSongWriteServiceModel);
    }
}

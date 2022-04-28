#region Usings

using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongSkippingDataService
    {
        Task<SkipSongReadServiceModel> GetCurrentSkipDataAsync(Guid playlistExternalId, Guid userExternalID);
    }
}

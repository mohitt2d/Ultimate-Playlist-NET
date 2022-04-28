#region Usings

using UltimatePlaylist.Services.Common.Models.Playlist;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Playlist
{
    public interface IUserPlaylistService
    {
        Task Set(Guid userExternalId, PlaylistReadServiceModel playlistReadServiceModel);

        Task<PlaylistReadServiceModel> Get(Guid userExternalId);
    }
}

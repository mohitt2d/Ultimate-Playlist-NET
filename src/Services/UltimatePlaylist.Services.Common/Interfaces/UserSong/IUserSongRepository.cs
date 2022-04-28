#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.UserSong
{
    public interface IUserSongRepository
    {
        public Task<IEnumerable<UserSongReadServiceModel>> GetUserPlaylistSongsAsync(Guid userExternalId, Guid playlistExternalId);
    }
}

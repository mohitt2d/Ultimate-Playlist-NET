#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.UserSong
{
    public interface IUserSongService
    {
        public Task<Result<IEnumerable<UserSongReadServiceModel>>> GetUserSongsForPlaylistAsync(Guid userExternalId, Guid playlistExternalId);
    }
}

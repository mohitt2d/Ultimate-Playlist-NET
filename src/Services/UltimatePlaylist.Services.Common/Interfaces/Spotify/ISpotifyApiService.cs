#region Usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Spotify
{
    public interface ISpotifyApiService
    {
        Task<Result<string>> GetSpotifyUserIdentity(string accessToken);

        Task<Result<SpotifyPlaylistsResponseModel>> FetchUserPlaylists(
            string accessToken);

        Task<Result<CreatePlaylistResponseModel>> CreatePlaylist(
            string accessToken,
            string userSpotifyIdentity);

        Task<Result> AddSongToPlaylistAsync(
            string accessToken,
            string playlistSpotifyId,
            string songSpotifyId);

        SpotifyClientConfigurationReadServiceModel GetSpotifyConfiguration();
    }
}

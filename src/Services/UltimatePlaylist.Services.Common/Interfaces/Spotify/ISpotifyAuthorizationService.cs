#region usings

using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Services.Common.Models.Spotify;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Spotify
{
    public interface ISpotifyAuthorizationService
    {
        Task<Result<SpotifyAuthorizationReadServiceModel>> ReceiveSpotifyTokens(SpotifyAuthorizationWriteServiceModel spotifyAuthorizationWriteServiceModel);

        Task<Result<SpotifyAuthorizationReadServiceModel>> RefreshSpotifyTokens(string refreshToken);
    }
}

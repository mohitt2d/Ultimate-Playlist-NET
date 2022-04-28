#region usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Common.Interfaces.Spotify
{
    public interface ISpotifyService
    {
        Task<Result> AuthorizeByCode(
            Guid userExternalId,
            SpotifyAuthorizationWriteServiceModel spotifyAuthorizationWriteServiceModel);

        Task<Result> AuthorizeWithTokens(
            Guid userExternalId,
            SpotifyAuthorizationWithTokensWriteServiceModel spotifyAuthorizationWithTokensWriteServiceModel);

        Task<Result> RemoveUserSpotifyDSP(DspType type, Guid userExternalId);

        Task<Result<EarnedTicketsReadServiceModel>> AddSongToUserSpotifyWithTicketsAsync(Guid userExternalId, AddSongToSpotifyWriteServiceModel addSongToSpotifyWriteServiceModel);

        Task<Result> AddSongToUserSpotifyWithoutTicketsAsync(Guid userExternalId, AddSongToSpotifyWriteServiceModel addSongToSpotifyWriteServiceModel);

        Task<Result<SpotifyClientConfigurationReadServiceModel>> GetSpotifyClientConfigurationAsync(Guid userExternalId);
    }
}

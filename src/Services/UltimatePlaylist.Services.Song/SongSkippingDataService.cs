#region Usings

using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Song
{
    public class SongSkippingDataService : ISongSkippingDataService
    {
        #region Private members

        private readonly Lazy<IRepository<UserPlaylistSongEntity>> UserPlaylistSongRepositoryProvider;

        private readonly PlaylistConfig PlaylistConfig;

        #endregion

        #region Constructor(s)

        public SongSkippingDataService(
            Lazy<IRepository<UserPlaylistSongEntity>> userPlaylistSongRepositoryProvider,
            IOptions<PlaylistConfig> playlistConfigOptions)
        {
            UserPlaylistSongRepositoryProvider = userPlaylistSongRepositoryProvider;
            PlaylistConfig = playlistConfigOptions.Value;
        }

        #endregion

        #region Properties

        private IRepository<UserPlaylistSongEntity> UserPlaylistSongRepository => UserPlaylistSongRepositoryProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<SkipSongReadServiceModel> GetCurrentSkipDataAsync(Guid playlistExternalId, Guid userExternalID)
        {
            var userPlaylistSkipedSongs = await UserPlaylistSongRepository.ListAsync(new UserPlaylistSongSpecification()
                .ByPlaylistExternalId(playlistExternalId)
                .ByUserExternalId(userExternalID)
                .OnlySkipped()
                .ByNewerThanDate(DateTime.UtcNow.Add(-PlaylistConfig.SongSkippingLimitTime))
                .WithPlaylist());

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                SkippedSongsCount = userPlaylistSkipedSongs.Count,
                CannotSkipSongTwice = false,
                SkippedSongsCountInLastHour = userPlaylistSkipedSongs.Where(c => c.SkipDate >= DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0))).Count(),
            };

            if (userPlaylistSkipedSongs.Count >= PlaylistConfig.SongSkippingLimit)
            {
                var oldestSkip = userPlaylistSkipedSongs.ToList().Where(s => s.SkipDate.HasValue).OrderBy(s => s.SkipDate).FirstOrDefault();
                skipSongReadServiceModel.ExpirationOfSkipLimitTimestamp = oldestSkip?.SkipDate is not null ? oldestSkip.SkipDate.Value.Add(PlaylistConfig.SongSkippingLimitTime)
                    : null;
                skipSongReadServiceModel.IsSkipLimitReached = true;
            }

            return skipSongReadServiceModel;
        }

        #endregion
    }
}

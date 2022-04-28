#region Usings

using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.Song
{
    public class SongSkippingService : ISongSkippingService
    {
        #region Private members

        private readonly Lazy<IRepository<UserPlaylistSongEntity>> UserPlaylistSongRepositoryProvider;

        private readonly PlaylistConfig PlaylistConfig;

        private readonly Lazy<IUserPlaylistService> UserPlaylistStoreProvider;

        private readonly Lazy<ISongSkippingDataService> SongSkippingDataServiceProvider;

        #endregion

        #region Constructor(s)

        public SongSkippingService(
            Lazy<IRepository<UserPlaylistSongEntity>> userPlaylistSongRepositoryProvider,
            IOptions<PlaylistConfig> playlistConfigOptions,
            Lazy<IUserPlaylistService> userPlaylistStoreProvider,
            Lazy<ISongSkippingDataService> songSkippingDataServiceProvider)
        {
            UserPlaylistSongRepositoryProvider = userPlaylistSongRepositoryProvider;
            PlaylistConfig = playlistConfigOptions.Value;
            UserPlaylistStoreProvider = userPlaylistStoreProvider;
            SongSkippingDataServiceProvider = songSkippingDataServiceProvider;
        }

        #endregion

        #region Properties

        private IRepository<UserPlaylistSongEntity> UserPlaylistSongRepository => UserPlaylistSongRepositoryProvider.Value;

        private IUserPlaylistService UserPlaylistStore => UserPlaylistStoreProvider.Value;

        private ISongSkippingDataService SongSkippingDataService => SongSkippingDataServiceProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<SkipSongReadServiceModel>> SkipSongAsync(SkipSongWriteServiceModel skipSongWriteServiceModel)
        {
            var skipData = await SongSkippingDataService.GetCurrentSkipDataAsync(skipSongWriteServiceModel.PlaylistExternalId, skipSongWriteServiceModel.UserExternalId);
            var result = Result.Success(skipData);

            if (!skipData.IsSkipLimitReached)
            {
                result = await result
                  .Bind(async _ => await GetUserPlaylistSongAsync(skipSongWriteServiceModel))
                  .TapIf(userPlaylistSong => userPlaylistSong.IsSkipped, userPlaylistSong => skipData.CannotSkipSongTwice = true)
                  .TapIf(userPlaylistSong => !userPlaylistSong.IsSkipped, async userPlaylistSong =>
                  {
                      userPlaylistSong.IsSkipped = true;
                      userPlaylistSong.SkipDate = DateTime.UtcNow;
                      userPlaylistSong.SecondsListened = skipSongWriteServiceModel.ActualListeningSecond;

                      await UserPlaylistSongRepository.UpdateAndSaveAsync(userPlaylistSong);

                      var userPlaylist = await UserPlaylistStore.Get(skipSongWriteServiceModel.UserExternalId);
                      var userPlaylistStoreSong = userPlaylist?.Songs.FirstOrDefault(s => s.ExternalId == userPlaylistSong.ExternalId);

                      if (userPlaylistStoreSong is not null)
                      {
                          userPlaylistStoreSong.IsSkipped = true;
                          var currentSongIndex = userPlaylist.Songs.IndexOf(userPlaylistStoreSong);

                          userPlaylist.CurrentSongExternalId = userPlaylist.Songs.Last().ExternalId == userPlaylistSong.ExternalId
                              ? userPlaylistSong.ExternalId
                              : userPlaylist.Songs[currentSongIndex + 1].ExternalId;

                          await UserPlaylistStore.Set(skipSongWriteServiceModel.UserExternalId, userPlaylist);
                      }

                      skipData.SkippedSongsCount++;
                      skipData.IsSongSkippedSuccessfully = true;
                      skipData.SkippedSongsCountInLastHour++;

                      if (skipData.SkippedSongsCount >= PlaylistConfig.SongSkippingLimit)
                      {
                          skipData.IsSkipLimitReached = true;
                      }
                      else
                      {
                          skipData.ExpirationOfSkipLimitTimestamp = null;
                      }
                  })
                  .Map(_ => skipData);
            }

            return result;
        }

        #endregion

        #region Private Method(s)

        private async Task<Result<UserPlaylistSongEntity>> GetUserPlaylistSongAsync(SkipSongWriteServiceModel skipSongWriteServiceModel)
        {
            var userPlaylistSong = await UserPlaylistSongRepository.FirstOrDefaultAsync(new UserPlaylistSongSpecification()
                .BySongExternalId(skipSongWriteServiceModel.SongExternalId)
                .ByPlaylistExternalId(skipSongWriteServiceModel.PlaylistExternalId)
                .ByUserExternalId(skipSongWriteServiceModel.UserExternalId)
                .WithPlaylist());

            return Result.SuccessIf(userPlaylistSong != null, userPlaylistSong, ErrorMessages.SongDoesNotExist);
        }

        #endregion
    }
}

#region Usings

using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Song.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.UserSong;

#endregion

namespace UltimatePlaylist.Services.UserSong
{
    public class UserSongHistoryService : IUserSongHistoryService
    {
        #region Private members

        private readonly Lazy<IReadOnlyRepository<User>> UserRepositoryProvider;

        private readonly Lazy<IRepository<UserSongHistoryEntity>> UserSongHistoryRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<SongEntity>> SongRepositoryProvider;

        #endregion

        #region Constructor(s)

        public UserSongHistoryService(
            Lazy<IReadOnlyRepository<User>> userRepositoryProvider,
            Lazy<IRepository<UserSongHistoryEntity>> userSongHistoryRepositoryProvider,
            Lazy<IReadOnlyRepository<SongEntity>> songRepositoryProvider)
        {
            UserRepositoryProvider = userRepositoryProvider;
            UserSongHistoryRepositoryProvider = userSongHistoryRepositoryProvider;
            SongRepositoryProvider = songRepositoryProvider;
        }

        #endregion

        #region Properties

        private IReadOnlyRepository<User> UserRepository => UserRepositoryProvider.Value;

        private IRepository<UserSongHistoryEntity> UserSongHistoryRepository => UserSongHistoryRepositoryProvider.Value;

        private IReadOnlyRepository<SongEntity> SongRepository => SongRepositoryProvider.Value;

        #endregion

        #region Public methods

        public async Task<Result<UserSongHistoryEntity>> GetOrAddUserSongHistoryAsync(
          Guid songExternalId,
          Guid userExternalId)
        {
            var existedUserSongHistory = await GetUserSongHistoryAsync(songExternalId, userExternalId);

            if (existedUserSongHistory.IsFailure && existedUserSongHistory.Error == ErrorType.CannotGetUserSongHistory.ToString())
            {
                User user = default;

                return await GetUserAsync(userExternalId)
                      .Tap(userEntity => user = userEntity)
                      .Bind(async _ => await GetSongAsync(songExternalId))
                      .Map(async song => await AddUserSongHistoryAsync(user, song));
            }

            return existedUserSongHistory;
        }

        #endregion

        #region Private methods

        // TODO: Move check for appleMusic to apple music service
        private async Task<Result<UserSongHistoryEntity>> GetUserSongHistoryAsync(
            Guid songExternalId,
            Guid userExternalId)
        {
            var userSongHistory = await UserSongHistoryRepository.FirstOrDefaultAsync(new UserSongHistorySpecification()
                .BySongExternalId(songExternalId)
                .ByUserExternalId(userExternalId)
                .OrderByCreatedDescending());

            return Result.SuccessIf(userSongHistory != null, userSongHistory, ErrorType.CannotGetUserSongHistory.ToString());
        }

        private async Task<Result<SongEntity>> GetSongAsync(Guid songExternalId)
        {
            var song = await SongRepository.FirstOrDefaultAsync(new SongSpecification()
                .ByExternalId(songExternalId));

            return Result.SuccessIf(song != null, song, ErrorType.CannotFindSong.ToString());
        }

        private async Task<UserSongHistoryEntity> AddUserSongHistoryAsync(
         User user,
         SongEntity song) => await UserSongHistoryRepository.AddAsync(new UserSongHistoryEntity()
         {
             Song = song,
             User = user,
         });

        private async Task<Result<User>> GetUserAsync(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification()
                .ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, ErrorType.UserDoesNotExist.ToString())
                .Map(() => user);
        }

        #endregion
    }
}
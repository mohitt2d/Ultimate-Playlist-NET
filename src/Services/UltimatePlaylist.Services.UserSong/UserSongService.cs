#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.UserSong;
using UltimatePlaylist.Services.Common.Models.Song;

#endregion

namespace UltimatePlaylist.Services.UserSong
{
    public class UserSongService : IUserSongService
    {
        #region Private fields

        private readonly Lazy<IUserSongRepository> UserSongRepositoryProvider;

        private readonly Lazy<IRepository<UserPlaylistEntity>> UserPlaylistRepositoryProvider;

        private readonly Lazy<IRepository<User>> UserRepositoryProvider;

        #endregion

        #region Constructor(s)

        public UserSongService(
            Lazy<IUserSongRepository> userSongRepositoryProvider,
            Lazy<IRepository<UserPlaylistEntity>> userPlaylistRepositoryProvider,
            Lazy<IRepository<User>> userRepositoryProvider)
        {
            UserSongRepositoryProvider = userSongRepositoryProvider;
            UserPlaylistRepositoryProvider = userPlaylistRepositoryProvider;
            UserRepositoryProvider = userRepositoryProvider;
        }

        #endregion

        #region Properties

        private IUserSongRepository UserSongRepository => UserSongRepositoryProvider.Value;

        private IRepository<UserPlaylistEntity> UserPlaylistRepository => UserPlaylistRepositoryProvider.Value;

        private IRepository<User> UserRepository => UserRepositoryProvider.Value;

        #endregion

        public async Task<Result<IEnumerable<UserSongReadServiceModel>>> GetUserSongsForPlaylistAsync(Guid userExternalId, Guid playlistExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification().ByExternalId(userExternalId));

            return await Result.FailureIf(user is null, ErrorType.UserDoesNotExist.ToString())
                .Ensure(async () => (await UserPlaylistRepository.FirstOrDefaultAsync(new UserPlaylistSpecification().ByExternalId(playlistExternalId))) is not null, ErrorType.PlaylistNotFound.ToString())
                .Map(async () => await UserSongRepository.GetUserPlaylistSongsAsync(userExternalId, playlistExternalId));
        }
    }
}

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Song.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Spotify;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Models.Dsp;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Request;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        #region Private members

        private readonly Lazy<ISpotifyAuthorizationService> SpotifyAuthorizationServiceProvider;

        private readonly Lazy<ISpotifyApiService> SpotifyApiServiceProvider;

        private readonly Lazy<ITicketService> TicketServiceProvider;

        private readonly Lazy<IRepository<UserDspEntity>> UserDspsRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<User>> UserRepositoryProvider;

        private readonly Lazy<IRepository<UserSongHistoryEntity>> UserSongHistoryRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<SongDSPEntity>> SongDSPRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<SongEntity>> SongRepositoryProvider;

        private readonly Lazy<IUserPlaylistService> UserPlaylistStoreProvider;

        private readonly Lazy<ILogger<SpotifyService>> LoggerProvider;

        private readonly Lazy<IMapper> MapperProvider;

        private readonly DSPConfig DSPConfig;

        private readonly Lazy<ISongAntibotService> SongAntibotServiceProvider;

        #endregion

        #region Constructor(s)

        public SpotifyService(
            Lazy<ISpotifyAuthorizationService> spotifyAuthorizationServiceProvider,
            Lazy<ISpotifyApiService> spotifyApiServiceProvider,
            Lazy<ITicketService> ticketServiceProvider,
            Lazy<IRepository<UserDspEntity>> userDspsRepositoryProvider,
            Lazy<IReadOnlyRepository<User>> userRepositoryProvider,
            Lazy<IRepository<UserSongHistoryEntity>> userSongHistoryRepositoryProvider,
            Lazy<IReadOnlyRepository<SongDSPEntity>> songDSPRepositoryProvider,
            Lazy<IReadOnlyRepository<SongEntity>> songRepositoryProvider,
            Lazy<IUserPlaylistService> userPlaylistStoreProvider,
            Lazy<ILogger<SpotifyService>> loggerProvider,
            Lazy<IMapper> mapperProvider,
            IOptions<DSPConfig> dspConfigOptions,
            Lazy<ISongAntibotService> songAntibotServiceProvider)
        {
            SpotifyAuthorizationServiceProvider = spotifyAuthorizationServiceProvider;
            SpotifyApiServiceProvider = spotifyApiServiceProvider;
            TicketServiceProvider = ticketServiceProvider;
            UserDspsRepositoryProvider = userDspsRepositoryProvider;
            UserRepositoryProvider = userRepositoryProvider;
            UserSongHistoryRepositoryProvider = userSongHistoryRepositoryProvider;
            SongDSPRepositoryProvider = songDSPRepositoryProvider;
            SongRepositoryProvider = songRepositoryProvider;
            UserPlaylistStoreProvider = userPlaylistStoreProvider;
            LoggerProvider = loggerProvider;
            MapperProvider = mapperProvider;
            DSPConfig = dspConfigOptions.Value;
            SongAntibotServiceProvider = songAntibotServiceProvider;
        }

        #endregion

        #region Properties

        private ISpotifyAuthorizationService SpotifyAuthorizationService => SpotifyAuthorizationServiceProvider.Value;

        private ISpotifyApiService SpotifyApiService => SpotifyApiServiceProvider.Value;

        private ITicketService TicketService => TicketServiceProvider.Value;

        private IRepository<UserDspEntity> UserDspsRepository => UserDspsRepositoryProvider.Value;

        private IReadOnlyRepository<User> UserRepository => UserRepositoryProvider.Value;

        private IRepository<UserSongHistoryEntity> UserSongHistoryRepository => UserSongHistoryRepositoryProvider.Value;

        private IReadOnlyRepository<SongDSPEntity> SongDSPRepository => SongDSPRepositoryProvider.Value;

        private IReadOnlyRepository<SongEntity> SongRepository => SongRepositoryProvider.Value;

        private IUserPlaylistService UserPlaylistStore => UserPlaylistStoreProvider.Value;

        private ILogger<SpotifyService> Logger => LoggerProvider.Value;

        private ISongAntibotService SongAntibotService => SongAntibotServiceProvider.Value;

        private IMapper Mapper => MapperProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result> AuthorizeByCode(
            Guid userExternalId,
            SpotifyAuthorizationWriteServiceModel spotifyAuthorizationWriteServiceModel)
        {
            User user = default;
            return await SpotifyAuthorizationService.ReceiveSpotifyTokens(spotifyAuthorizationWriteServiceModel)
                .Check(async _ => await GetUser(userExternalId)
                    .Tap(existUser => user = existUser))
                .Check(async result => await StoreReceivedSpotifyTokens(user, result));
        }

        public async Task<Result> AuthorizeWithTokens(
            Guid userExternalId,
            SpotifyAuthorizationWithTokensWriteServiceModel spotifyAuthorizationWithTokensWriteServiceModel)
        {
            var mapped = Mapper.Map<SpotifyAuthorizationReadServiceModel>(spotifyAuthorizationWithTokensWriteServiceModel);
            return await GetUser(userExternalId)
                .Check(async user => await StoreReceivedSpotifyTokens(user, mapped));
        }

        public async Task<Result> RemoveUserSpotifyDSP(DspType type, Guid userExternalId)
        {
            return await Result.Success()
                .Tap(async () => await UserDspsRepository.DeleteAsync(new UserDspSpecification().ByUserExternalId(userExternalId).ByType(type)));
        }

        public async Task<Result> AddSongToUserSpotifyWithoutTicketsAsync(Guid userExternalId, AddSongToSpotifyWriteServiceModel addSongToSpotifyWriteServiceModel)
        {
            return await AddSongToUserSpotifyAsync(userExternalId, addSongToSpotifyWriteServiceModel);
        }

        public async Task<Result<EarnedTicketsReadServiceModel>> AddSongToUserSpotifyWithTicketsAsync(Guid userExternalId, AddSongToSpotifyWriteServiceModel addSongToSpotifyWriteServiceModel)
        {
            return await AddSongToUserSpotifyAsync(userExternalId, addSongToSpotifyWriteServiceModel)
                .Bind(async () => await TicketService.AddUserTicketAsync(
                    userExternalId,
                    new AddTicketWriteServiceModel()
                    {
                        ExternalId = addSongToSpotifyWriteServiceModel.SongExternalId,
                        PlaylistExternalId = addSongToSpotifyWriteServiceModel.PlaylistExternalId,
                        EarnedType = TicketEarnedType.AddedToSpotify,
                        Type = TicketType.Daily,
                    }))
                 .Tap(async _ => await SongAntibotService.ResetCounterAsync(userExternalId))
                 .Tap(async _ => await UpdatePlaylistStore(userExternalId, addSongToSpotifyWriteServiceModel.SongExternalId));
        }

        public async Task<Result<SpotifyClientConfigurationReadServiceModel>> GetSpotifyClientConfigurationAsync(Guid userExternalId)
        {
            return await GetUser(userExternalId)
                .Map(_ => SpotifyApiService.GetSpotifyConfiguration());
        }

        #endregion

        #region Private Method(s)

        private async Task<Result> AddSongToUserSpotifyAsync(Guid userExternalId, AddSongToSpotifyWriteServiceModel addSongToSpotifyWriteServiceModel)
        {
            UserDspEntity userDspEntity = default;
            UserSongHistoryEntity userSongHistoryEntity = default;

            return await GetOrAddUserSongHistoryAsync(addSongToSpotifyWriteServiceModel.SongExternalId, userExternalId)
                .Tap(userSongHistory => userSongHistoryEntity = userSongHistory)
                .Bind(async userSongHistory => await GetUserDsp(userExternalId, DspType.Spotify))
                .Tap(userDsp => userDspEntity = userDsp)
                .Bind(async _ => await RefreshTokenAndStoreToDatabase(userDspEntity.SpotifyRefreshToken, userExternalId))
                .Check(async _ => await GetUserSpotifyProfileAndSaveIdentityInDatabase(userDspEntity)
                        .Tap(userSpotifyIdentity => userDspEntity.UserSpotifyIdentity = userSpotifyIdentity))
                .Check(async _ => await CreatePlaylistOrRestoreAsync(userDspEntity)
                        .Tap(playlistSpotifyId => userDspEntity.UserPlaylistId = playlistSpotifyId))
                    .Tap(async () => await SaveUserSpotifyIdentityAndSpotifyPlaylist(userDspEntity))
                .Bind(async _ => await GetSpotifySongDspAsync(addSongToSpotifyWriteServiceModel.SongExternalId))
                .Bind(async songDSP => await AddSongToPlaylistAsync(userDspEntity, songDSP, userSongHistoryEntity));
        }

        private async Task<Result<UserDspEntity>> RefreshTokenAndStoreToDatabase(
            string refreshToken,
            Guid userExternalId)
        {
            User user = default;
            return await SpotifyAuthorizationService.RefreshSpotifyTokens(refreshToken)
                .Check(async _ => await GetUser(userExternalId)
                    .Tap(existUser => user = existUser))
                .Bind(async result => await StoreRefreshedAccessToken(user, result));
        }

        private async Task<Result<User>> GetUser(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification()
                .ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, ErrorType.CannotFindUser.ToString())
                .Map(() => user);
        }

        private async Task<Result> StoreReceivedSpotifyTokens(User user, SpotifyAuthorizationReadServiceModel spotifyAuthorizationReadServiceModel)
        {
            if (user is null)
            {
                return Result.Failure(ErrorType.UserDoesNotExist.ToString());
            }

            var userDsp = await UserDspsRepository.FirstOrDefaultAsync(new UserDspSpecification()
                .ByUserExternalId(user.ExternalId)
                .ByType(DspType.Spotify)
                .WithUser()
                .OrderByCreatedDescending());

            spotifyAuthorizationReadServiceModel.TokenType = "Bearer";

            if (userDsp is null)
            {
                var newUserDsp = new UserDspEntity()
                {
                    Type = DspType.Spotify,
                    IsActive = true,
                    SpotifyAccessToken = spotifyAuthorizationReadServiceModel.AccessToken,
                    SpotifyRefreshToken = spotifyAuthorizationReadServiceModel.RefreshToken,
                    SpotifyTokenType = spotifyAuthorizationReadServiceModel.TokenType,
                    SpotifyScopes = !string.IsNullOrEmpty(spotifyAuthorizationReadServiceModel.Scope)
                        ? spotifyAuthorizationReadServiceModel.Scope
                        : DspConst.SpotifyScopes,
                    AccessTokenExpirationDate = spotifyAuthorizationReadServiceModel.AccessTokenExpirationDate,
                    User = user,
                    UserId = user.Id,
                };

                var addedEntity = await UserDspsRepository.AddAsync(newUserDsp);

                return Result.SuccessIf(addedEntity != null, ErrorType.CannotStoreUserDsp.ToString());
            }

            userDsp.IsActive = true;
            userDsp.SpotifyAccessToken = spotifyAuthorizationReadServiceModel.AccessToken;
            userDsp.SpotifyRefreshToken = spotifyAuthorizationReadServiceModel.RefreshToken;
            userDsp.SpotifyTokenType = spotifyAuthorizationReadServiceModel.TokenType;
            userDsp.SpotifyScopes = !string.IsNullOrEmpty(spotifyAuthorizationReadServiceModel.Scope)
                        ? spotifyAuthorizationReadServiceModel.Scope
                        : DspConst.SpotifyScopes;
            userDsp.AccessTokenExpirationDate = spotifyAuthorizationReadServiceModel.AccessTokenExpirationDate;

            var updatedEntity = await UserDspsRepository.UpdateAndSaveAsync(userDsp);

            return Result.SuccessIf(updatedEntity != null, ErrorType.CannotUpdateUserDsp.ToString());
        }

        private async Task<Result<UserDspEntity>> StoreRefreshedAccessToken(User user, SpotifyAuthorizationReadServiceModel spotifyAuthorizationReadServiceModel)
        {
            if (user is null)
            {
                return Result.Failure<UserDspEntity>(ErrorType.UserDoesNotExist.ToString());
            }

            var userDsp = await UserDspsRepository.FirstOrDefaultAsync(new UserDspSpecification()
                .ByUserExternalId(user.ExternalId)
                .ByType(DspType.Spotify)
                .OrderByCreatedDescending());

            if (userDsp is null)
            {
                return Result.Failure<UserDspEntity>(ErrorType.UserDspDoesNotExist.ToString());
            }

            userDsp.SpotifyAccessToken = spotifyAuthorizationReadServiceModel.AccessToken;
            userDsp.SpotifyRefreshToken = !string.IsNullOrEmpty(spotifyAuthorizationReadServiceModel.RefreshToken)
                ? spotifyAuthorizationReadServiceModel.RefreshToken
                : userDsp.SpotifyRefreshToken;
            userDsp.AccessTokenExpirationDate = spotifyAuthorizationReadServiceModel.AccessTokenExpirationDate;

            var updatedEntity = await UserDspsRepository.UpdateAndSaveAsync(userDsp);

            return Result.SuccessIf(updatedEntity != null && updatedEntity.Updated != null, updatedEntity, ErrorType.CannotUpdateUserDsp.ToString());
        }

        private async Task<Result<UserDspEntity>> GetUserDsp(
            Guid userExternalId,
            DspType dspType)
        {
            var userDsp = await UserDspsRepository.FirstOrDefaultAsync(new UserDspSpecification()
                .ByUserExternalId(userExternalId)
                .ByType(dspType)
                .WithUser()
                .OrderByCreatedDescending());

            return Result.SuccessIf(userDsp != null, userDsp, ErrorType.UserDspDoesNotExist.ToString());
        }

        private async Task<Result<string>> GetUserSpotifyProfileAndSaveIdentityInDatabase(UserDspEntity userDsp)
        {
            return await SpotifyApiService.GetSpotifyUserIdentity(userDsp.SpotifyAccessToken)
                .Bind(spotifyIdentity =>
                {
                    return Result.SuccessIf(!string.IsNullOrEmpty(spotifyIdentity), spotifyIdentity, ErrorType.CannotUpdateUserDsp.ToString());
                });
        }

        private async Task<Result<string>> CreatePlaylistOrRestoreAsync(UserDspEntity userDsp)
        {
            var userSpotifyPlaylists = await SpotifyApiService.FetchUserPlaylists(userDsp.SpotifyAccessToken);

            if (userSpotifyPlaylists.IsSuccess)
            {
                var ultimatePlaylist = userSpotifyPlaylists.Value.Items.FirstOrDefault(s => s.Name == DSPConfig.DefaultPlayListName);

                if (ultimatePlaylist != null)
                {
                    return ultimatePlaylist.Id;
                }
            }

            return await SpotifyApiService.CreatePlaylist(
                userDsp.SpotifyAccessToken,
                userDsp.UserSpotifyIdentity)
                .Bind(createdPlaylist =>
                {
                    return Result.SuccessIf(createdPlaylist != null && createdPlaylist.Id != null, createdPlaylist.Id, ErrorType.CannotUpdateUserDsp.ToString());
                });
        }

        private async Task<Result> SaveUserSpotifyIdentityAndSpotifyPlaylist(UserDspEntity userDspEntity)
        {
            var updated = await UserDspsRepository.UpdateAndSaveAsync(userDspEntity);

            return Result.SuccessIf(updated != null && !string.IsNullOrEmpty(updated.UserSpotifyIdentity) && !string.IsNullOrEmpty(updated.UserPlaylistId), ErrorType.CannotUpdateUserDsp.ToString());
        }

        private async Task<Result<UserSongHistoryEntity>> GetOrAddUserSongHistoryAsync(
            Guid songExternalId,
            Guid userExternalId)
        {
            var existedUserSongHistory = await GetUserSongHistoryAsync(songExternalId, userExternalId);

            if (existedUserSongHistory.IsFailure && existedUserSongHistory.Error == ErrorType.CannotGetUserSongHistory.ToString())
            {
                User user = default;

                return await GetUserAsync(userExternalId)
                    .Tap(userEntity => user = userEntity)
                    .Bind(async _ => await GetSong(songExternalId))
                    .Bind(async song => await AddUserSongHistoryAsync(user, song));
            }

            return existedUserSongHistory;
        }

        private async Task<Result<User>> GetUserAsync(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification()
                .ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, ErrorType.CannotFindUser.ToString())
                .Map(() => user);
        }

        private async Task<Result<SongEntity>> GetSong(Guid songExternalId)
        {
            var song = await SongRepository.FirstOrDefaultAsync(new SongSpecification()
                .ByExternalId(songExternalId));

            return Result.SuccessIf(song != null, song, ErrorType.CannotFindSong.ToString());
        }

        private async Task<Result<UserSongHistoryEntity>> AddUserSongHistoryAsync(
           User user,
           SongEntity songEntity)
        {
            var userSongHistoryToAdd = new UserSongHistoryEntity()
            {
                SongId = songEntity.Id,
                UserId = user.Id,
            };

            var addedEntity = await UserSongHistoryRepository.AddAsync(userSongHistoryToAdd);

            return Result.SuccessIf(addedEntity != null, addedEntity, ErrorType.CannotAddUserTicketHistory.ToString());
        }

        private async Task<Result<UserSongHistoryEntity>> GetUserSongHistoryAsync(
            Guid songExternalId,
            Guid userExternalId)
        {
            var userSongHistory = await UserSongHistoryRepository.FirstOrDefaultAsync(new UserSongHistorySpecification()
                .BySongExternalId(songExternalId)
                .ByUserExternalId(userExternalId)
                .OrderByCreatedDescending());

            return Result.SuccessIf(userSongHistory != null, userSongHistory, ErrorType.CannotGetUserSongHistory.ToString())
                .Check(_ => Result.SuccessIf(!userSongHistory.IsAddedToSpotify, userSongHistory, ErrorType.CannotAddSongCurrentlyAddedToSpotify.ToString()));
        }

        private async Task<Result<SongDSPEntity>> GetSpotifySongDspAsync(
            Guid songExternalId)
        {
            var songDsp = await SongDSPRepository.FirstOrDefaultAsync(new SongDSPSpecification()
                .BySongExternalId(songExternalId)
                .ByDSPType(DspType.Spotify)
                .WithSong()
                .OrderByCreatedDescending());

            return Result.SuccessIf(songDsp != null, songDsp, ErrorType.CannotGetSongDSP.ToString());
        }

        private async Task<Result> AddSongToPlaylistAsync(
            UserDspEntity userDsp,
            SongDSPEntity songDSP,
            UserSongHistoryEntity userSongHistory)
        {
            return await SpotifyApiService.AddSongToPlaylistAsync(
                userDsp.SpotifyAccessToken,
                userDsp.UserPlaylistId,
                songDSP.SongDspId)
                .Bind(async () =>
                {
                    userSongHistory.IsAddedToSpotify = true;
                    var updated = await UserSongHistoryRepository.UpdateAndSaveAsync(userSongHistory);
                    return Result.SuccessIf(updated != null && updated.Updated != null, updated, ErrorType.CannotUpdateStatusForAddedSongToSpotify.ToString());
                });
        }

        private async Task UpdatePlaylistStore(Guid userExternalId, Guid songExternalId)
        {
            var userPlaylist = await UserPlaylistStore.Get(userExternalId);

            if (userPlaylist != null)
            {
                if (userPlaylist.State == PlaylistState.NotStartedYet)
                {
                    userPlaylist.State = PlaylistState.InProgress;
                }

                var song = userPlaylist.Songs.First(s => s.ExternalId == songExternalId);
                var songIndex = userPlaylist.Songs.IndexOf(song);

                userPlaylist.CurrentSongExternalId = songExternalId;
                userPlaylist.Songs[songIndex].IsAddedToSpotify = true;

                await UserPlaylistStore.Set(userExternalId, userPlaylist);
            }
        }

        #endregion
    }
}

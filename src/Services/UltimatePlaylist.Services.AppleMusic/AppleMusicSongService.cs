#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Song.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Client.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Interfaces.UserSong;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.AppleMusic
{
    public class AppleMusicSongService : IAppleMusicSongService
    {
        #region Private members

        private readonly Lazy<ITicketService> TicketServiceProvider;

        private readonly Lazy<IRepository<UserSongHistoryEntity>> UserSongHistoryRepositoryProvider;

        private readonly Lazy<IReadOnlyRepository<SongDSPEntity>> SongDSPRepositoryProvider;

        private readonly Lazy<IRepository<UserDspEntity>> UserDspsRepositoryProvider;

        private readonly Lazy<IUserPlaylistService> UserPlaylistStoreProvider;

        private readonly Lazy<IAppleMusicPlaylistService> AppleMusicPlaylistServiceProvider;

        private readonly Lazy<IAppleMusicSongClientService> AppleMusicSongClientServiceProvider;

        private readonly Lazy<IUserSongHistoryService> UserSongHistoryServiceProvider;

        private readonly Lazy<ISongAntibotService> SongAntibotServiceProvider;

        #endregion

        #region Constructor(s)

        public AppleMusicSongService(
            Lazy<ITicketService> ticketServiceProvider,
            Lazy<IRepository<UserSongHistoryEntity>> userSongHistoryRepositoryProvider,
            Lazy<IReadOnlyRepository<SongDSPEntity>> songDSPRepositoryProvider,
            Lazy<IRepository<UserDspEntity>> userDspsRepositoryProvider,
            Lazy<IUserPlaylistService> userPlaylistStoreProvider,
            Lazy<IAppleMusicPlaylistService> appleMusicPlaylistServiceProvider,
            Lazy<IAppleMusicSongClientService> appleMusicSongClientServiceProvider,
            Lazy<IUserSongHistoryService> userSongHistoryServiceProvider,
            Lazy<ISongAntibotService> songAntibotServiceProvider)
        {
            TicketServiceProvider = ticketServiceProvider;
            UserSongHistoryRepositoryProvider = userSongHistoryRepositoryProvider;
            SongDSPRepositoryProvider = songDSPRepositoryProvider;
            UserDspsRepositoryProvider = userDspsRepositoryProvider;
            UserPlaylistStoreProvider = userPlaylistStoreProvider;
            AppleMusicPlaylistServiceProvider = appleMusicPlaylistServiceProvider;
            AppleMusicSongClientServiceProvider = appleMusicSongClientServiceProvider;
            UserSongHistoryServiceProvider = userSongHistoryServiceProvider;
            SongAntibotServiceProvider = songAntibotServiceProvider;
        }

        #endregion

        #region Properties

        private ITicketService TicketService => TicketServiceProvider.Value;

        private IRepository<UserSongHistoryEntity> UserSongHistoryRepository => UserSongHistoryRepositoryProvider.Value;

        private IReadOnlyRepository<SongDSPEntity> SongDSPRepository => SongDSPRepositoryProvider.Value;

        private IRepository<UserDspEntity> UserDspsRepository => UserDspsRepositoryProvider.Value;

        private IAppleMusicPlaylistService AppleMusicPlaylistService => AppleMusicPlaylistServiceProvider.Value;

        private IAppleMusicSongClientService AppleMusicSongClientService => AppleMusicSongClientServiceProvider.Value;

        private IUserPlaylistService UserPlaylistStore => UserPlaylistStoreProvider.Value;

        private IUserSongHistoryService UserSongHistoryService => UserSongHistoryServiceProvider.Value;

        private ISongAntibotService SongAntibotService => SongAntibotServiceProvider.Value;

        #endregion

        #region Public Methods

        public async Task<Result<EarnedTicketsReadServiceModel>> AddSongToAppleMusicWithTicketAsync(Guid userExternalId, AddSongToAppleMusicWriteServiceModel addSongToAppleMusicWriteServiceModel)
        {
            return await AddSongToAppleMusicAsync(userExternalId, addSongToAppleMusicWriteServiceModel)
                .Bind(async songDsp => await TicketService.AddUserTicketAsync(
                    userExternalId,
                    new AddTicketWriteServiceModel()
                    {
                        ExternalId = addSongToAppleMusicWriteServiceModel.SongExternalId,
                        PlaylistExternalId = addSongToAppleMusicWriteServiceModel.PlaylistExternalId,
                        EarnedType = TicketEarnedType.AddedToAppleMusic,
                        Type = TicketType.Daily,
                    }))
                .Tap(async _ => await SongAntibotService.ResetCounterAsync(userExternalId))
                .Tap(async serviceModel => await UpdatePlaylistStore(userExternalId, addSongToAppleMusicWriteServiceModel.SongExternalId));
        }

        public async Task<Result> AddSongToAppleMusicWithoutTicketAsync(Guid userExternalId, AddSongToAppleMusicWriteServiceModel addSongToAppleMusicWriteServiceModel)
        {
            return await AddSongToAppleMusicAsync(userExternalId, addSongToAppleMusicWriteServiceModel);
        }

        #endregion

        #region Private methods

        private async Task<Result<SongDSPEntity>> AddSongToAppleMusicAsync(
           Guid userExternalId,
           AddSongToAppleMusicWriteServiceModel addSongToAppleMusicWriteServiceModel)
        {
            UserSongHistoryEntity userSongHistoryEntity = default;
            UserDspEntity userDspEntity = default;

            return await GetUserDsp(userExternalId)
                .Tap(userDsp => userDspEntity = userDsp)
                .Bind(async userDsp => await UserSongHistoryService.GetOrAddUserSongHistoryAsync(addSongToAppleMusicWriteServiceModel.SongExternalId, userExternalId))
                .Ensure(userSongHistory => userSongHistory != null && !userSongHistory.IsAddedToAppleMusic, ErrorType.CannotAddSongAlreadyAddedToAppleMusic.ToString())
                .Tap(userSongHistory => userSongHistoryEntity = userSongHistory)
                .Check(async userSongHistory => await AppleMusicPlaylistService.CreateOrRestorePlaylistAsync(userExternalId))
                .Bind(async userSongHistory => await GetAppleMusicSongDspAsync(addSongToAppleMusicWriteServiceModel.SongExternalId))
                .Check(async songDSP => await AddSongToPlaylistAsync(userDspEntity, songDSP, userSongHistoryEntity, userExternalId));
        }

        private async Task<Result<SongDSPEntity>> GetAppleMusicSongDspAsync(
            Guid songExternalId)
        {
            var songDsp = await SongDSPRepository.FirstOrDefaultAsync(new SongDSPSpecification()
                .BySongExternalId(songExternalId)
                .ByDSPType(DspType.AppleMusic)
                .WithSong()
                .OrderByCreatedDescending());

            return Result.SuccessIf(songDsp != null && songDsp.SongDspId != null, songDsp, ErrorType.CannotGetSongDSP.ToString());
        }

        private async Task<Result> AddSongToPlaylistAsync(
          UserDspEntity userDsp,
          SongDSPEntity songDSP,
          UserSongHistoryEntity userSongHistory,
          Guid userExternalId)
        {
            return await AppleMusicSongClientService.AddSongToPlaylistAsync(
                userDsp.AppleUserToken,
                AppleMusicResurceType.Playlists,
                userDsp.UserPlaylistId,
                songDSP.SongDspId,
                userExternalId)
                .Bind(async () =>
                {
                    userSongHistory.IsAddedToAppleMusic = true;
                    var updated = await UserSongHistoryRepository.UpdateAndSaveAsync(userSongHistory);
                    return Result.SuccessIf(updated != null && updated.Updated != null && updated.IsAddedToAppleMusic, updated, ErrorType.CannotUpdateStatusForAddedSongToAppleMusic.ToString());
                });
        }

        private async Task UpdatePlaylistStore(Guid userExternalId, Guid songExternalId)
        {
            var userPlaylist = await UserPlaylistStore.Get(userExternalId);

            if (userPlaylist != null)
            {
                var song = userPlaylist.Songs.First(s => s.ExternalId == songExternalId);
                var songIndex = userPlaylist.Songs.IndexOf(song);

                userPlaylist.CurrentSongExternalId = songExternalId;
                userPlaylist.Songs[songIndex].IsAddedToAppleMusic = true;

                await UserPlaylistStore.Set(userExternalId, userPlaylist);

                if (userPlaylist.State == PlaylistState.NotStartedYet)
                {
                    userPlaylist.State = PlaylistState.InProgress;
                }
            }
        }

        private async Task<Result<UserDspEntity>> GetUserDsp(
            Guid userExternalId)
        {
            var userDsp = await UserDspsRepository.FirstOrDefaultAsync(new UserDspSpecification()
                .ByUserExternalId(userExternalId)
                .ByType(DspType.AppleMusic)
                .ByActive()
                .WithUser()
                .OrderByCreatedDescending());

            return Result.SuccessIf(userDsp?.AppleUserToken != null && userDsp.IsActive, userDsp, ErrorMessages.DisconnectedFromAppleMusic);
        }

        #endregion
    }
}

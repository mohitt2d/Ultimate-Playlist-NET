#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.Services.Song
{
    public class SongRatingService : ISongRatingService
    {
        #region Private members

        private readonly Lazy<ITicketService> TicketServiceProvider;

        private readonly Lazy<IReadOnlyRepository<User>> UserProvider;

        private readonly Lazy<IRepository<UserPlaylistEntity>> UserPlaylistRepositoryProvider;

        private readonly Lazy<IRepository<UserPlaylistSongEntity>> UserPlaylistSongRepositoryProvider;

        private readonly Lazy<IUserPlaylistService> UserPlaylistStoreProvider;

        private readonly Lazy<ITicketStatsService> TicketStatsServiceProvider;

        private readonly Lazy<ISongAntibotService> SongAntibotServiceProvider;

        #endregion

        #region Constructor(s)

        public SongRatingService(
            Lazy<ITicketService> ticketServiceProvider,
            Lazy<IReadOnlyRepository<User>> userProvider,
            Lazy<IRepository<UserPlaylistEntity>> userPlaylistRepositoryProvider,
            Lazy<IRepository<UserPlaylistSongEntity>> userPlaylistSongRepositoryProvider,
            Lazy<IUserPlaylistService> userPlaylistStoreProvider,
            Lazy<ITicketStatsService> ticketStatsServiceProvider,
            Lazy<ISongAntibotService> songAntibotServiceProvider)
        {
            TicketServiceProvider = ticketServiceProvider;
            UserProvider = userProvider;
            UserPlaylistRepositoryProvider = userPlaylistRepositoryProvider;
            UserPlaylistSongRepositoryProvider = userPlaylistSongRepositoryProvider;
            UserPlaylistStoreProvider = userPlaylistStoreProvider;
            TicketStatsServiceProvider = ticketStatsServiceProvider;
            SongAntibotServiceProvider = songAntibotServiceProvider;
        }

        #endregion

        #region Properties

        private ITicketService TicketService => TicketServiceProvider.Value;

        private IReadOnlyRepository<User> UserRepository => UserProvider.Value;

        private IRepository<UserPlaylistEntity> UserPlaylistRepository => UserPlaylistRepositoryProvider.Value;

        private IRepository<UserPlaylistSongEntity> UserPlaylistSongRepository => UserPlaylistSongRepositoryProvider.Value;

        private IUserPlaylistService UserPlaylistStore => UserPlaylistStoreProvider.Value;

        private ITicketStatsService TicketStatsService => TicketStatsServiceProvider.Value;

        private ISongAntibotService SongAntibotService => SongAntibotServiceProvider.Value;

        #endregion

        #region Public Method(s)

        public async Task<Result<EarnedTicketsReadServiceModel>> RateSongAsync(Guid userExternalId, RateSongWriteServiceModel rateSongWriteServiceModel)
        {
            UserPlaylistEntity userPlaylist = default;

            return await GetUser(userExternalId)
                .Bind(async _ => await GetPlaylist(rateSongWriteServiceModel.PlaylistExternalId)
                .Tap(userPlaylistEntity => userPlaylist = userPlaylistEntity)
                .Bind(userPlaylistEntity => GetPlaylistSong(userPlaylist, rateSongWriteServiceModel.ExternalId))
                .Tap(async userPlaylistSong => await AddSongRatingAsync(userPlaylistSong, rateSongWriteServiceModel.Rating))
                .Bind(async _ => await TicketService.AddUserTicketAsync(
                    userExternalId,
                    new AddTicketWriteServiceModel()
                    {
                        EarnedType = TicketEarnedType.Rating,
                        ExternalId = rateSongWriteServiceModel.ExternalId,
                        PlaylistExternalId = rateSongWriteServiceModel.PlaylistExternalId,
                        Type = TicketType.Daily,
                    })
                .Tap(async _ => await UpdateUserPlaylistState(userExternalId, rateSongWriteServiceModel.ExternalId, rateSongWriteServiceModel.Rating, userPlaylist))))
                .Tap(async _ => await SongAntibotService.ResetCounterAsync(userExternalId))
                .Bind(async _ => await TicketStatsService.UserTicketStatsAsync(userExternalId))
                .Map(ticketsStats => new EarnedTicketsReadServiceModel() { LatestEarnedTickets = ticketsStats.TicketsAmountForTodayDrawing });
        }

        #endregion

        #region Private Method(s)

        private async Task UpdateUserPlaylistState(Guid userExternalId, Guid songExternalId, int rating, UserPlaylistEntity userPlaylistEntity)
        {
            var userPlaylist = await UserPlaylistStore.Get(userExternalId);

            if (userPlaylist is not null)
            {
                if (userPlaylist.State == PlaylistState.NotStartedYet)
                {
                    userPlaylist.State = PlaylistState.InProgress;
                }

                var song = userPlaylist.Songs.First(s => s.ExternalId == songExternalId);
                var songIndex = userPlaylist.Songs.IndexOf(song);

                userPlaylist.CurrentSongExternalId = songExternalId;
                userPlaylist.Songs[songIndex].UserRating = rating;
                userPlaylist.Songs.ForEach(s => s.IsCurrent = false);
                userPlaylist.Songs[songIndex].IsCurrent = true;

                await UserPlaylistStore.Set(userExternalId, userPlaylist);
            }

            if (userPlaylistEntity.State == PlaylistState.NotStartedYet)
            {
                userPlaylistEntity.State = PlaylistState.InProgress;
            }

            foreach (var userPlaylistSong in userPlaylistEntity.UserPlaylistSongs)
            {
                userPlaylistSong.IsCurrent = userPlaylistSong.Song.ExternalId == songExternalId;
            }

            await UserPlaylistRepository.UpdateAndSaveAsync(userPlaylistEntity);
        }

        private async Task<Result<User>> GetUser(Guid userExternalId)
        {
            var user = await UserRepository.FirstOrDefaultAsync(new UserSpecification()
                .ByExternalId(userExternalId));

            return Result.SuccessIf(user != null, user, ErrorType.CannotFindUser.ToString());
        }

        private Result<UserPlaylistSongEntity> GetPlaylistSong(
            UserPlaylistEntity userPlaylist,
            Guid songExternalId)
        {
            var userPlaylistSong = userPlaylist.UserPlaylistSongs.FirstOrDefault(s => s.Song.ExternalId == songExternalId);

            return Result.SuccessIf(userPlaylistSong != null, userPlaylistSong, ErrorType.CannotFindSong.ToString());
        }

        private async Task AddSongRatingAsync(
            UserPlaylistSongEntity userPlaylistSongEntity,
            int rating)
        {
            userPlaylistSongEntity.Rating = rating;
            await UserPlaylistSongRepository.UpdateAndSaveAsync(userPlaylistSongEntity);
        }

        private async Task<Result<UserPlaylistEntity>> GetPlaylist(Guid playlistExternalId)
        {
            var playlist = await UserPlaylistRepository.FirstOrDefaultAsync(new UserPlaylistSpecification()
                .ByExternalId(playlistExternalId)
                .OrderByCreatedDescending()
                .WithSongs());

            return Result.SuccessIf(playlist != null, playlist, ErrorType.PlaylistNotFound.ToString());
        }

        #endregion
    }
}

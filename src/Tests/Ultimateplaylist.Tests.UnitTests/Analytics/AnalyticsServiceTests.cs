#region Usings

using AutoMapper;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Analytics;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Models.Analytics;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Tests.UnitTests.Common;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Analytics
{
    public class AnalyticsServiceTests
    {
        private readonly Mock<IRepository<User>> UserRepositoryMock;
        private readonly Mock<ITicketService> TicketServiceMock;
        private readonly Mock<ITicketStatsService> TicketStatsServiceMock;
        private readonly Mock<IUserPlaylistService> UserPlaylistStoreMock;
        private readonly Mock<ISongSkippingService> SongSkippingServiceMock;
        private readonly Mock<IPlaylistService> PlaylistServiceMock;
        private readonly Mock<IRepository<UserPlaylistEntity>> UserPlaylistRepositoryMock;
        private readonly Mock<IRepository<UserPlaylistSongEntity>> UserPlaylistSongRepositoryMock;
        private readonly Mock<IMapper> MockedMapper;
        private readonly IAnalyticsService AnalyticsService;
        private readonly Mock<ISongSkippingDataService> SongSkippingDataServiceMock;
        private readonly Mock<ISongAntibotService> SongAntibotServiceMock;
        private readonly Mock<IPlaylistSQLRepository> PlaylistSQLRepositoryMock;

        public AnalyticsServiceTests()
        {
            UserRepositoryMock = new Mock<IRepository<User>>();
            TicketServiceMock = new Mock<ITicketService>();
            TicketStatsServiceMock = new Mock<ITicketStatsService>();
            UserPlaylistStoreMock = new Mock<IUserPlaylistService>();
            SongSkippingServiceMock = new Mock<ISongSkippingService>();
            PlaylistServiceMock = new Mock<IPlaylistService>();
            UserPlaylistRepositoryMock = new Mock<IRepository<UserPlaylistEntity>>();
            UserPlaylistSongRepositoryMock = new Mock<IRepository<UserPlaylistSongEntity>>();
            SongAntibotServiceMock = new Mock<ISongAntibotService>();
            PlaylistSQLRepositoryMock = new Mock<IPlaylistSQLRepository>();

            MockedMapper = new Mock<IMapper>();
            SongSkippingDataServiceMock = new Mock<ISongSkippingDataService>();
            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 0,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            SongSkippingServiceMock.Setup(x => x.SkipSongAsync(It.IsAny<SkipSongWriteServiceModel>()))
                .Returns(Task.FromResult(Result.Success(skipSongReadServiceModel)));

            AnalyticsService = new AnalyticsService(
                new Lazy<IRepository<User>>(UserRepositoryMock.Object),
                new Lazy<ITicketStatsService>(TicketStatsServiceMock.Object),
                new Lazy<ITicketService>(TicketServiceMock.Object),
                new Lazy<IUserPlaylistService>(UserPlaylistStoreMock.Object),
                new Lazy<ISongSkippingService>(SongSkippingServiceMock.Object),
                new Lazy<IPlaylistService>(PlaylistServiceMock.Object),
                new Lazy<IRepository<UserPlaylistEntity>>(UserPlaylistRepositoryMock.Object),
                new Lazy<IRepository<UserPlaylistSongEntity>>(UserPlaylistSongRepositoryMock.Object),
                new Lazy<IMapper>(MockedMapper.Object),
                new Lazy<ISongSkippingDataService>(SongSkippingDataServiceMock.Object),
                new Lazy<Microsoft.Extensions.Logging.ILogger<AnalyticsService>>(),
                new Lazy<ISongAntibotService>(SongAntibotServiceMock.Object),
                new Lazy<IPlaylistSQLRepository>(PlaylistSQLRepositoryMock.Object));
        }

        [Fact]
        public async Task Analytics_Skip_Song_User_Does_Not_Exist()
        {
            // Mock
            var userServiceModel = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            var saveAnalyticsDataWriteServiceModel = AnalyticsTestDataProvider.GetSaveAnalyticsDataWriteServiceModel(AnalitycsEventType.SkipSong, actualListeningSecond: 100);
            var userPlaylistReadServiceModel = AnalyticsTestDataProvider.GetPlaylistReadServiceModelWithRandomExternalIds();

            PlaylistServiceMock.Setup(playlistService =>
                playlistService.GetTodaysPlaylist(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(userPlaylistReadServiceModel));

            // Act
            var result = await AnalyticsService.SaveAnalitycsDataAsync(userServiceModel.ExternalId, saveAnalyticsDataWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ErrorMessages.UserDoesNotExist);
        }

        [Fact]
        public async Task Analytics_Skip_Song_Does_Not_Exist()
        {
            // Mock
            var userServiceModel = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            var saveAnalyticsDataWriteServiceModel = AnalyticsTestDataProvider.GetSaveAnalyticsDataWriteServiceModel(AnalitycsEventType.SkipSong, actualListeningSecond: 100);
            var userPlaylistReadServiceModel = AnalyticsTestDataProvider.GetPlaylistReadServiceModelWithRandomExternalIds();

            UserRepositoryMock.Setup(userRepository =>
                userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(Task.FromResult(userServiceModel));

            PlaylistServiceMock.Setup(playlistService =>
                playlistService.GetTodaysPlaylist(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(userPlaylistReadServiceModel));

            SongSkippingServiceMock.Setup(x => x.SkipSongAsync(It.IsAny<SkipSongWriteServiceModel>()))
                .Returns(Task.FromResult(Result.Failure<SkipSongReadServiceModel>(ErrorMessages.SongDoesNotExist)));

            // Act
            var result = await AnalyticsService.SaveAnalitycsDataAsync(userServiceModel.ExternalId, saveAnalyticsDataWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ErrorMessages.SongDoesNotExist);
        }

        [Fact]
        public async Task Analytics_Three_Songs_WithoutSkip()
        {
            // Mock
            var userServiceModel = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            var userPlaylistReadServiceModel = AnalyticsTestDataProvider.GetPlaylistReadServiceModelWithRandomExternalIds();
            var saveAnalyticsDataWriteServiceModel = AnalyticsTestDataProvider.GetSaveAnalyticsDataWriteServiceModel(
                AnalitycsEventType.EntireSong,
                actualListeningSecond: 100,
                songExternalId: userPlaylistReadServiceModel.Songs.LastOrDefault().ExternalId);

            PlaylistServiceMock.Setup(playlistService =>
                playlistService.GetTodaysPlaylist(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(userPlaylistReadServiceModel));

            UserPlaylistSongRepositoryMock.Setup(playlistService =>
                playlistService.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(async () => await Task.FromResult(new UserPlaylistSongEntity() { ExternalId = saveAnalyticsDataWriteServiceModel.SongExternalId }));

            UserRepositoryMock.Setup(userServiceModel => userServiceModel.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(userServiceModel));

            TicketStatsServiceMock.Setup(x => x.UserTicketStatsAsync(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(Result.Success(AnalyticsTestDataProvider.GetLastEarnedNotDisplayedTicketsReadServiceModel())));

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 0,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(saveAnalyticsDataWriteServiceModel.PlaylistExternalId, userServiceModel.ExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            MockedMapper.Setup(mapper => mapper.Map<AnalyticsLastEarnedTicketsReadServiceModel>(skipSongReadServiceModel))
                .Returns(AnalyticsTestDataProvider.GetAnalyticsEarnedTicketsReadServiceModel(2));

            // Act
            var result = await AnalyticsService.SaveAnalitycsDataAsync(userServiceModel.ExternalId, saveAnalyticsDataWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Add_Ticket_For_Three_Songs_Without_Skip_While_Any_Skippped()
        {
            // Mock
            var userServiceModel = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            var userPlaylistReadServiceModel = AnalyticsTestDataProvider.GetPlaylistReadServiceModelWithRandomExternalIds(numbersOfSongs: 4);
            var saveAnalyticsDataWriteServiceModel = AnalyticsTestDataProvider.GetSaveAnalyticsDataWriteServiceModel(
                AnalitycsEventType.EntireSong,
                actualListeningSecond: 100,
                songExternalId: userPlaylistReadServiceModel.Songs.LastOrDefault().ExternalId);

            userPlaylistReadServiceModel.Songs
                .Where(x => x.ExternalId != saveAnalyticsDataWriteServiceModel.SongExternalId)
                .ForEach(x => x.IsSkipped = true);

            PlaylistServiceMock.Setup(playlistService =>
                playlistService.GetTodaysPlaylist(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(userPlaylistReadServiceModel));

            UserPlaylistSongRepositoryMock.Setup(playlistService => playlistService.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(async () => await Task.FromResult(new UserPlaylistSongEntity() { ExternalId = saveAnalyticsDataWriteServiceModel.SongExternalId }));

            UserRepositoryMock.Setup(userServiceModel => userServiceModel.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(userServiceModel));

            TicketStatsServiceMock.Setup(x => x.UserTicketStatsAsync(userServiceModel.ExternalId))
                .Returns(async () => await Task.FromResult(Result.Success(AnalyticsTestDataProvider.GetLastEarnedNotDisplayedTicketsReadServiceModel())));

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 0,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(saveAnalyticsDataWriteServiceModel.PlaylistExternalId, userServiceModel.ExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            // Act
            var result = await AnalyticsService.SaveAnalitycsDataAsync(userServiceModel.ExternalId, saveAnalyticsDataWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}

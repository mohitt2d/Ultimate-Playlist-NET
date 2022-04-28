#region Usings

using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Song;
using UltimatePlaylist.Tests.UnitTests.Common;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Analytics
{
    public class SongSkippingServiceTests
    {
        private readonly Mock<IRepository<UserPlaylistSongEntity>> UserPlaylistSongRepositoryMock;
        private readonly Mock<IUserPlaylistService> UserPlaylistService;
        private readonly Mock<ISongSkippingDataService> SongSkippingDataServiceMock;
        private readonly Mock<IOptions<PlaylistConfig>> PlaylistConfigMock;
        private readonly ISongSkippingService SongSkippingService;
        private readonly PlaylistConfig PlayListConfig;

        public SongSkippingServiceTests()
        {
            UserPlaylistSongRepositoryMock = new Mock<IRepository<UserPlaylistSongEntity>>();

            PlayListConfig = new PlaylistConfig()
            {
                SongSkippingLimit = 6,
                SongSkippingLimitTime = new TimeSpan(0, 1, 0),
            };
            PlaylistConfigMock = new Mock<IOptions<PlaylistConfig>>();
            PlaylistConfigMock.Setup(ap => ap.Value).Returns(PlayListConfig);
            UserPlaylistService = new Mock<IUserPlaylistService>();
            SongSkippingDataServiceMock = new Mock<ISongSkippingDataService>();

            SongSkippingService = new SongSkippingService(
                new Lazy<IRepository<UserPlaylistSongEntity>>(UserPlaylistSongRepositoryMock.Object),
                PlaylistConfigMock.Object,
                new Lazy<IUserPlaylistService>(UserPlaylistService.Object),
                new Lazy<ISongSkippingDataService>(SongSkippingDataServiceMock.Object));
        }

        [Fact]
        public async Task Skip_Song_Successfully()
        {
            // Mock
            var skipSongWriteServiceModel = new SkipSongWriteServiceModel()
            {
                UserExternalId = Guid.NewGuid(),
                PlaylistExternalId = Guid.NewGuid(),
                SongExternalId = Guid.NewGuid(),
            };

            var userPlaylistSong = new UserPlaylistSongEntity()
            {
                ExternalId = Guid.NewGuid(),
            };

            var userPlaylistSongs = new List<UserPlaylistSongEntity>();

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = true,
                SkippedSongsCount = 1,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(skipSongWriteServiceModel.PlaylistExternalId, skipSongWriteServiceModel.UserExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            var userPlaylistReadServiceModel = AnalyticsTestDataProvider.GetPlaylistReadServiceModelWithRandomExternalIds();

            UserPlaylistService.Setup(userPlaylist => userPlaylist.Get(skipSongWriteServiceModel.UserExternalId)).Returns(Task.FromResult(userPlaylistReadServiceModel));

            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.ListAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult((IReadOnlyList<UserPlaylistSongEntity>)userPlaylistSongs));
            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult(userPlaylistSong));

            // Act
            var result = await SongSkippingService.SkipSongAsync(skipSongWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.IsSongSkippedSuccessfully.Should().BeTrue();
            result.Value.SkippedSongsCount.Should().Be(skipSongReadServiceModel.SkippedSongsCount++);
            result.Value.IsSkipLimitReached.Should().BeFalse();
            result.Value.CannotSkipSongTwice.Should().BeFalse();
            result.Value.ExpirationOfSkipLimitTimestamp.Should().BeNull();
        }

        [Fact]
        public async Task Skip_Song_Filed_Cannot_Skip_Song_Twice()
        {
            // Mock
            var skipSongWriteServiceModel = new SkipSongWriteServiceModel()
            {
                UserExternalId = Guid.NewGuid(),
                PlaylistExternalId = Guid.NewGuid(),
                SongExternalId = Guid.NewGuid(),
            };

            var userPlaylistSong = new UserPlaylistSongEntity()
            {
                ExternalId = Guid.NewGuid(),
                IsSkipped = true,
            };

            var userPlaylistSongs = new List<UserPlaylistSongEntity>();

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 0,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(skipSongWriteServiceModel.PlaylistExternalId, skipSongWriteServiceModel.UserExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.ListAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult((IReadOnlyList<UserPlaylistSongEntity>)userPlaylistSongs));
            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult(userPlaylistSong));

            // Act
            var result = await SongSkippingService.SkipSongAsync(skipSongWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.IsSongSkippedSuccessfully.Should().BeFalse();
            result.Value.SkippedSongsCount.Should().Be(0);
            result.Value.IsSkipLimitReached.Should().BeFalse();
            result.Value.CannotSkipSongTwice.Should().BeTrue();
            result.Value.ExpirationOfSkipLimitTimestamp.Should().BeNull();
        }

        [Fact]
        public async Task Skip_Song_Filed_Cannot_Get_User_Playlist_Song()
        {
            // Mock
            var skipSongWriteServiceModel = new SkipSongWriteServiceModel()
            {
                UserExternalId = Guid.NewGuid(),
                PlaylistExternalId = Guid.NewGuid(),
                SongExternalId = Guid.NewGuid(),
            };

            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult((UserPlaylistSongEntity)null));

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = false,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 0,
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(skipSongWriteServiceModel.PlaylistExternalId, skipSongWriteServiceModel.UserExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            // Act
            var result = await SongSkippingService.SkipSongAsync(skipSongWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ErrorMessages.SongDoesNotExist);
        }

        [Fact]
        public async Task Skip_Song_Reached_Limit()
        {
            // Mock
            var skipDate = DateTime.UtcNow;
            var skipSongWriteServiceModel = new SkipSongWriteServiceModel()
            {
                UserExternalId = Guid.NewGuid(),
                PlaylistExternalId = Guid.NewGuid(),
                SongExternalId = Guid.NewGuid(),
            };

            var userPlaylistSong = new UserPlaylistSongEntity()
            {
                ExternalId = Guid.NewGuid(),
            };

            var userPlaylistSongs = new List<UserPlaylistSongEntity>()
            {
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-50)),
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-40)),
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-30)),
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-20)),
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-10)),
                GetUserPlaylistSongEntity(skipDate.AddMinutes(-5)),
            };

            var skipSongReadServiceModel = new SkipSongReadServiceModel()
            {
                CannotSkipSongTwice = false,
                IsSkipLimitReached = true,
                IsSongSkippedSuccessfully = false,
                SkippedSongsCount = 6,
                ExpirationOfSkipLimitTimestamp = skipDate.AddMinutes(-50).Add(PlayListConfig.SongSkippingLimitTime),
            };

            SongSkippingDataServiceMock.Setup(x => x.GetCurrentSkipDataAsync(skipSongWriteServiceModel.PlaylistExternalId, skipSongWriteServiceModel.UserExternalId))
                .Returns(Task.FromResult(skipSongReadServiceModel));

            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.ListAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult((IReadOnlyList<UserPlaylistSongEntity>)userPlaylistSongs));
            UserPlaylistSongRepositoryMock.Setup(userPlaylistSongRepository =>
                userPlaylistSongRepository.FirstOrDefaultAsync(It.IsAny<UserPlaylistSongSpecification>()))
                .Returns(Task.FromResult(userPlaylistSong));

            // Act
            var result = await SongSkippingService.SkipSongAsync(skipSongWriteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.IsSongSkippedSuccessfully.Should().BeFalse();
            result.Value.SkippedSongsCount.Should().Be(6);
            result.Value.IsSkipLimitReached.Should().BeTrue();
            result.Value.CannotSkipSongTwice.Should().BeFalse();
            result.Value.ExpirationOfSkipLimitTimestamp.Should().Be(skipDate.AddMinutes(-50).Add(PlayListConfig.SongSkippingLimitTime));
        }

        private static UserPlaylistSongEntity GetUserPlaylistSongEntity(DateTime? skipDate = null)
        {
            return new UserPlaylistSongEntity()
            {
                ExternalId = Guid.NewGuid(),
                SkipDate = skipDate,
            };
        }
    }
}

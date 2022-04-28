#region Usings

using System;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Spotify;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Spotify;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Spotify
{
    public class SpotifyServiceTests
    {
        [Fact]
        public async Task Authorization_Success_With_Adding_New_Dsp()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedUser = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            UserDspEntity mockedUserDsp = default;

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
                spotifyAuthorizationService =>
                    spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
                .Returns(async () => await Task.FromResult(mockedReadServiceModel));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.AddAsync(It.IsAny<UserDspEntity>()))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid() }));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            var songAntibotService = new Mock<ISongAntibotService>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Authorization_Success_With_Updating_Exist_Dsp()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedUser = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            UserDspEntity mockedUserDsp = new UserDspEntity() { ExternalId = Guid.NewGuid() };

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
                spotifyAuthorizationService =>
                    spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
                .Returns(async () => await Task.FromResult(mockedReadServiceModel));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.UpdateAndSaveAsync(It.IsAny<UserDspEntity>(), true))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            var songAntibotService = new Mock<ISongAntibotService>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Authorization_Failure_With_Receiving_Response_From_Dsp()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedUser = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            UserDspEntity mockedUserDsp = default;

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
               spotifyAuthorizationService =>
                   spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
               .Returns(async () => await Task.FromResult(Result.Failure<SpotifyAuthorizationReadServiceModel>(ErrorType.FailedToAuthorizeSpotify.ToString())));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.AddAsync(It.IsAny<UserDspEntity>()))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid() }));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var songAntibotService = new Mock<ISongAntibotService>();

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.FailedToAuthorizeSpotify.ToString());
        }

        [Fact]
        public async Task Authorization_Failure_Can_Not_Find_User()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            User mockedUser = default;

            UserDspEntity mockedUserDsp = default;

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
                spotifyAuthorizationService =>
                    spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
                .Returns(async () => await Task.FromResult(mockedReadServiceModel));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.AddAsync(It.IsAny<UserDspEntity>()))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid() }));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            var songAntibotService = new Mock<ISongAntibotService>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.CannotFindUser.ToString());
        }

        [Fact]
        public async Task Authorization_Failure_Can_Not_Store_User_Dsp()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedUser = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            UserDspEntity mockedUserDsp = default;

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
                spotifyAuthorizationService =>
                    spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
                .Returns(async () => await Task.FromResult(mockedReadServiceModel));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            var songAntibotService = new Mock<ISongAntibotService>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.CannotStoreUserDsp.ToString());
        }

        [Fact]
        public async Task Authorization_Failure_Can_Not_Update_User_Dsp()
        {
            // Mock
            var mockedWrtiteServiceModel = new SpotifyAuthorizationWriteServiceModel()
            {
                Code = "123124152512521",
            };

            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedUser = new User()
            {
                ExternalId = Guid.NewGuid(),
            };

            UserDspEntity mockedUserDsp = new UserDspEntity() { ExternalId = Guid.NewGuid() };

            var spotifyAuthorizationServiceMock = new Mock<ISpotifyAuthorizationService>();
            spotifyAuthorizationServiceMock.Setup(
                spotifyAuthorizationService =>
                    spotifyAuthorizationService.ReceiveSpotifyTokens(mockedWrtiteServiceModel))
                .Returns(async () => await Task.FromResult(mockedReadServiceModel));

            var spotifyApiServiceMock = new Mock<ISpotifyApiService>();

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(mockedUserDsp));

            var userRepositoryMock = new Mock<IReadOnlyRepository<User>>();
            userRepositoryMock.Setup(
                userRepository =>
                    userRepository.FirstOrDefaultAsync(It.IsAny<UserSpecification>()))
                .Returns(async () => await Task.FromResult(mockedUser));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();

            var songDSPRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();

            var songRepositoryMock = new Mock<IReadOnlyRepository<SongEntity>>();

            var ticketServiceMock = new Mock<ITicketService>();

            var userPlaylistStoreMock = new Mock<IUserPlaylistService>();

            var mockedLogger = new Mock<ILogger<SpotifyService>>();

            var mockedMapper = new Mock<IMapper>();

            var songAntibotService = new Mock<ISongAntibotService>();

            DSPConfig dspConfig = new DSPConfig();
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(dspConfig);

            var spotifyAutorizationServiceMock = new SpotifyService(
                new Lazy<ISpotifyAuthorizationService>(spotifyAuthorizationServiceMock.Object),
                new Lazy<ISpotifyApiService>(spotifyApiServiceMock.Object),
                new Lazy<ITicketService>(ticketServiceMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<User>>(userRepositoryMock.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDSPRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongEntity>>(songRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStoreMock.Object),
                new Lazy<ILogger<SpotifyService>>(mockedLogger.Object),
                new Lazy<IMapper>(mockedMapper.Object),
                dspConfigMock.Object,
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.AuthorizeByCode(It.IsAny<Guid>(), mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.CannotUpdateUserDsp.ToString());
        }
    }
}

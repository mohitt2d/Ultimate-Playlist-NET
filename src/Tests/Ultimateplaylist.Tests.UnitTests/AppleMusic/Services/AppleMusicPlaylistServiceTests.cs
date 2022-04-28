#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Const;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.AppleMusic;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic.Client;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Request;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Responses;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.AppleMusic.Services
{
    public class AppleMusicPlaylistServiceTests
    {
        private DSPConfig DSPConfig = new DSPConfig()
        {
            DefaultPlayListDescription = "TEST",
            DefaultPlayListName = "TEST",
        };

        [Fact]
        public async Task RestorePlaylist_Success()
        {
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(DSPConfig);

            var playListReturnId = "playlistId";

            var userAppleDSP = new UserDspEntity()
            {
                AppleUserToken = "Token",
                IsActive = true,
            };

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(userAppleDSP));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.UpdateAndSaveAsync(userAppleDSP, true))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid(), UserPlaylistId = playListReturnId }));

            var applePlaylistResponse = new AppleDataResponseRoot<AppleMusicPlaylistDataResponseModel>()
            {
                Data = new List<AppleMusicPlaylistDataResponseModel>()
                {
                    new AppleMusicPlaylistDataResponseModel()
                    {
                       Attributes = new AppleMusicPlaylistAttributesResponseModel() { Name = "TEST" },
                       Id = playListReturnId,
                    },
                },
            };

            var appleMusicPlaylistClientService = new Mock<IAppleMusicPlaylistClientService>();

            appleMusicPlaylistClientService.Setup(
                appleMusicPlaylistClientService =>
                    appleMusicPlaylistClientService.FetchPlaylistsAsync(userAppleDSP.AppleUserToken, It.IsAny<Guid>(), It.IsAny<AppleMusicPageOptions>()))
                .Returns(async () => await Task.FromResult(applePlaylistResponse));

            var appleMusicPlaylistService = new AppleMusicPlaylistService(
                new Lazy<IAppleMusicPlaylistClientService>(appleMusicPlaylistClientService.Object),
                dspConfigMock.Object,
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object));

            // Act
            var result = await appleMusicPlaylistService.CreateOrRestorePlaylistAsync(Guid.NewGuid());

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(applePlaylistResponse.Data.FirstOrDefault()?.Id);
        }

        [Fact]
        public async Task CreatePlaylist_Success()
        {
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(DSPConfig);

            var playListReturnId = "playlistId";

            var userAppleDSP = new UserDspEntity()
            {
                AppleUserToken = "Token",
                IsActive = true,
            };

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(userAppleDSP));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.UpdateAndSaveAsync(userAppleDSP, true))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid(), UserPlaylistId = playListReturnId }));

            var applePlaylistFetchResponse = new AppleDataResponseRoot<AppleMusicPlaylistDataResponseModel>()
            {
                Data = new List<AppleMusicPlaylistDataResponseModel>(),
            };

            var applePlaylistCreateResponse = new AppleMusicPlaylistDataResponseModel()
            {
                Attributes = new AppleMusicPlaylistAttributesResponseModel() { Name = "TEST" },
                Id = playListReturnId,
            };

            var appleMusicPlaylistClientService = new Mock<IAppleMusicPlaylistClientService>();

            appleMusicPlaylistClientService.Setup(
                appleMusicPlaylistClientService =>
                    appleMusicPlaylistClientService.FetchPlaylistsAsync(userAppleDSP.AppleUserToken, It.IsAny<Guid>(), It.IsAny<AppleMusicPageOptions>()))
                .Returns(async () => await Task.FromResult(applePlaylistFetchResponse));

            appleMusicPlaylistClientService.Setup(
               appleMusicPlaylistClientService =>
                   appleMusicPlaylistClientService.CreateNewLibraryResources<AppleMusicPlaylistDataResponseModel>(
                       userAppleDSP.AppleUserToken,
                       It.IsAny<Guid>(),
                       AppleMusicResurceType.Playlists,
                       It.IsAny<AppleMusicCreatePlaylistRequestModel>()))
               .Returns(async () => await Task.FromResult(applePlaylistCreateResponse));

            var appleMusicPlaylistService = new AppleMusicPlaylistService(
                new Lazy<IAppleMusicPlaylistClientService>(appleMusicPlaylistClientService.Object),
                dspConfigMock.Object,
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object));

            // Act
            var result = await appleMusicPlaylistService.CreateOrRestorePlaylistAsync(Guid.NewGuid());

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(applePlaylistCreateResponse.Id);
        }

        [Fact]
        public async Task CreatePlaylist_FailNoDsp()
        {
            var dspConfigMock = new Mock<IOptions<DSPConfig>>();
            dspConfigMock.Setup(ap => ap.Value).Returns(DSPConfig);

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult<UserDspEntity>(null));

            var appleMusicPlaylistClientService = new Mock<IAppleMusicPlaylistClientService>();

            var appleMusicPlaylistService = new AppleMusicPlaylistService(
                new Lazy<IAppleMusicPlaylistClientService>(appleMusicPlaylistClientService.Object),
                dspConfigMock.Object,
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object));

            // Act
            var result = await appleMusicPlaylistService.CreateOrRestorePlaylistAsync(Guid.NewGuid());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ErrorMessages.DisconnectedFromAppleMusic);
        }
    }
}

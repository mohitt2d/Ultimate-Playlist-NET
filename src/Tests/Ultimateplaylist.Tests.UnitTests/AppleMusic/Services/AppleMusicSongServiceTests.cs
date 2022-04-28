#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Song.Specifications;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Repositories.Interfaces;
using UltimatePlaylist.Services.AppleMusic;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Client.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Playlist;
using UltimatePlaylist.Services.Common.Interfaces.Song;
using UltimatePlaylist.Services.Common.Interfaces.Ticket;
using UltimatePlaylist.Services.Common.Interfaces.UserSong;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.AppleMusic.Responses;
using UltimatePlaylist.Services.Common.Models.Dsp;
using UltimatePlaylist.Services.Common.Models.Playlist;
using UltimatePlaylist.Services.Common.Models.Song;
using UltimatePlaylist.Services.Common.Models.Ticket;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.AppleMusic.Services
{
    public class AppleMusicSongServiceTests
    {
        private readonly string ApppleUserToken = "AppleUserToken";
        private readonly int EarnedTickets = 3;

        [Fact]
        public async Task AddSongToAppleMusic_Success()
        {
            var songId = "songId";
            var playlistId = "playlistId";
            var userExternalId = Guid.NewGuid();

            var addSongToAppleMusic = new AddSongToAppleMusicWriteServiceModel()
            {
                PlaylistExternalId = Guid.NewGuid(),
                SongExternalId = Guid.NewGuid(),
            };

            var userAppleDSP = new UserDspEntity()
            {
                AppleUserToken = "Token",
                IsActive = true,
            };

            var ticketService = new Mock<ITicketService>();

            var songAntibotService = new Mock<ISongAntibotService>();

            ticketService.Setup(
                ticketService =>
                    ticketService.AddUserTicketAsync(userExternalId, It.IsAny<AddTicketWriteServiceModel>()))
                .Returns(async () => await Task.FromResult(new EarnedTicketsReadServiceModel() { LatestEarnedTickets = EarnedTickets }));

            var userDspsRepositoryMock = new Mock<IRepository<UserDspEntity>>();
            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.FirstOrDefaultAsync(It.IsAny<UserDspSpecification>()))
               .Returns(async () => await Task.FromResult(userAppleDSP));

            userDspsRepositoryMock.Setup(
               userDspsRepository =>
                   userDspsRepository.UpdateAndSaveAsync(userAppleDSP, true))
               .Returns(async () => await Task.FromResult(new UserDspEntity() { ExternalId = Guid.NewGuid(), UserPlaylistId = playlistId, AppleUserToken = ApppleUserToken }));

            var songDspRepositoryMock = new Mock<IReadOnlyRepository<SongDSPEntity>>();
            songDspRepositoryMock.Setup(
               songDspRepositoryMock =>
                   songDspRepositoryMock.FirstOrDefaultAsync(It.IsAny<SongDSPSpecification>()))
               .Returns(async () => await Task.FromResult(new SongDSPEntity() { ExternalId = addSongToAppleMusic.SongExternalId, DspType = DspType.AppleMusic, SongDspId = "123" }));

            var userSongHistoryRepositoryMock = new Mock<IRepository<UserSongHistoryEntity>>();
            userSongHistoryRepositoryMock.Setup(
               userSongHistoryRepositoryMock =>
                   userSongHistoryRepositoryMock.UpdateAndSaveAsync(It.IsAny<UserSongHistoryEntity>(), true))
               .Returns(async () => await Task.FromResult(new UserSongHistoryEntity() { ExternalId = Guid.NewGuid(), Updated = DateTime.UtcNow, IsAddedToAppleMusic = true }));

            var userPlaylistServiceModel = new PlaylistReadServiceModel()
            {
                State = PlaylistState.InProgress,
                Songs = new List<UserSongReadServiceModel>()
                {
                    new UserSongReadServiceModel() { ExternalId = addSongToAppleMusic.SongExternalId },
                },
            };

            var userPlaylistStore = new Mock<IUserPlaylistService>();

            userPlaylistStore.Setup(
                userPlaylistStore =>
                    userPlaylistStore.Get(userExternalId))
                .Returns(async () => await Task.FromResult(userPlaylistServiceModel));

            userPlaylistStore.Setup(
               userPlaylistStore =>
                   userPlaylistStore.Set(userExternalId, It.IsAny<PlaylistReadServiceModel>()));

            var appleMusicPlaylistService = new Mock<IAppleMusicPlaylistService>();

            var applePlaylistCreateResponse = new AppleMusicPlaylistDataResponseModel()
            {
                Attributes = new AppleMusicPlaylistAttributesResponseModel() { Name = "TEST" },
                Id = playlistId,
            };

            appleMusicPlaylistService.Setup(
                appleMusicPlaylistService =>
                    appleMusicPlaylistService.CreateOrRestorePlaylistAsync(userExternalId))
                .Returns(async () => await Task.FromResult(applePlaylistCreateResponse));

            var appleMusicPlaylistClientService = new Mock<IAppleMusicSongClientService>();

            appleMusicPlaylistClientService.Setup(
                appleMusicPlaylistClientService =>
                    appleMusicPlaylistClientService.AddSongToPlaylistAsync(userAppleDSP.AppleUserToken, AppleMusicResurceType.Playlists, playlistId, songId, userExternalId))
                .Returns(async () => await Task.FromResult(Result.Success()));

            var userSongHistoryService = new Mock<IUserSongHistoryService>();

            userSongHistoryService.Setup(
                userSongHistoryService =>
                    userSongHistoryService.GetOrAddUserSongHistoryAsync(addSongToAppleMusic.SongExternalId, userExternalId))
                .Returns(async () => await Task.FromResult(new UserSongHistoryEntity() { ExternalId = Guid.NewGuid() }));

            var appleMusicSongService = new AppleMusicSongService(
                new Lazy<ITicketService>(ticketService.Object),
                new Lazy<IRepository<UserSongHistoryEntity>>(userSongHistoryRepositoryMock.Object),
                new Lazy<IReadOnlyRepository<SongDSPEntity>>(songDspRepositoryMock.Object),
                new Lazy<IRepository<UserDspEntity>>(userDspsRepositoryMock.Object),
                new Lazy<IUserPlaylistService>(userPlaylistStore.Object),
                new Lazy<IAppleMusicPlaylistService>(appleMusicPlaylistService.Object),
                new Lazy<IAppleMusicSongClientService>(appleMusicPlaylistClientService.Object),
                new Lazy<IUserSongHistoryService>(userSongHistoryService.Object),
                new Lazy<ISongAntibotService>(songAntibotService.Object));

            // Act
            var result = await appleMusicSongService.AddSongToAppleMusicWithTicketAsync(userExternalId, addSongToAppleMusic);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.LatestEarnedTickets.Should().Be(EarnedTickets);
        }
    }
}

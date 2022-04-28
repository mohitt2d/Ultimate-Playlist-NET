#region Usings

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.AppleMusic;
using UltimatePlaylist.Services.Common.AppleMusic.Enums;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.AppleMusic.Services.Clients
{
    public class AppleMusicSongClientServiceTests
    {
        [Fact]
        public async Task AddSongToPlaylist_Success()
        {
            var mockedLogger = new Mock<ILogger<AppleMusicBaseClient>>();

            var appleMusicTokenService = new Mock<IAppleMusicTokenService>();

            var appleMusicConnectionService = new Mock<IAppleMusicConnectionService>();

            appleMusicTokenService.Setup(
                appleMusicTokenService =>
                    appleMusicTokenService.CreateAppleMusicToken())
                .Returns(() => "token");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://apple.com/"),
            };

            var mockedHttpclientFactory = new Mock<IHttpClientFactory>();
            mockedHttpclientFactory.Setup(x => x.CreateClient(Config.AppleHttpClient)).Returns(httpClient);

            var appleMusicSongService = new AppleMusicSongClientService(
                mockedHttpclientFactory.Object,
                new Lazy<IAppleMusicTokenService>(appleMusicTokenService.Object),
                new Lazy<IAppleMusicConnectionService>(appleMusicConnectionService.Object),
                new Lazy<ILogger<AppleMusicBaseClient>>(mockedLogger.Object));

            // Act
            var result = await appleMusicSongService.AddSongToPlaylistAsync(
                "user-token",
                AppleMusicResurceType.Playlists,
                "playlistId",
                "appleMusicSongId",
                Guid.NewGuid());

            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}

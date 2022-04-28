#region Usings

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;
using UltimatePlaylist.Services.Spotify;
using Xunit;

#endregion

namespace UltimatePlaylist.Tests.UnitTests.Users.Services.Spotify
{
    public class SotifyAuthorizationServiceTests
    {
        [Fact]
        public async Task Authorization_Success()
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

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

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
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   {
                      access_token = mockedReadServiceModel.AccessToken,
                      expires_in = 3600,
                      refresh_token = mockedReadServiceModel.RefreshToken,
                      scope = mockedReadServiceModel.Scope,
                      token_type = mockedReadServiceModel.TokenType,
                   })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.ReceiveSpotifyTokens(mockedWrtiteServiceModel);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.AccessToken.Should().Be(mockedReadServiceModel.AccessToken);
        }

        [Fact]
        public async Task Authorization_From_Spotify_Api_Failure()
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

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.BadRequest,
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   { })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.ReceiveSpotifyTokens(mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.FailedToAuthorizeSpotify.ToString());
        }

        [Fact]
        public async Task Authorization_From_Spotify_Api_With_Empty_Response()
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

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

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
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   { })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.ReceiveSpotifyTokens(mockedWrtiteServiceModel);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.FailedToAuthorizeSpotify.ToString());
        }

        [Fact]
        public async Task Refresh_Token_Success()
        {
            // Mock
            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

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
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   {
                       access_token = mockedReadServiceModel.AccessToken,
                       expires_in = 3600,
                       refresh_token = mockedReadServiceModel.RefreshToken,
                       scope = mockedReadServiceModel.Scope,
                       token_type = mockedReadServiceModel.TokenType,
                   })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.RefreshSpotifyTokens(It.IsAny<string>());

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.AccessToken.Should().Be(mockedReadServiceModel.AccessToken);
        }

        [Fact]
        public async Task Refresh_Token_From_Spotify_Api_Failure()
        {
            // Mock
            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.BadRequest,
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   { })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.RefreshSpotifyTokens(It.IsAny<string>());

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.FailedToRefreshSpotifyAccessToken.ToString());
        }

        [Fact]
        public async Task Refresh_Token_From_Spotify_Api_With_Empty_Response()
        {
            // Mock
            var mockedReadServiceModel = new SpotifyAuthorizationReadServiceModel()
            {
                AccessToken = "AAABBBB12345",
                AccessTokenExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = "1234567890",
                Scope = "playlist-modify-public playlist-read-collaborative",
                TokenType = "Bearer",
            };

            var mockedLogger = new Mock<ILogger<SpotifyAuthorizationService>>();

            var mockedConfig = new Mock<IOptions<SpotifyConfig>>();
            mockedConfig.Setup(ap => ap.Value).Returns(new SpotifyConfig()
            {
                ClientId = "11111",
                ClientSecret = "22222",
                RedirectUri = "https://google.com",
                AuthorizationUrl = "https://accounts.spotify.com/api/test",
            });

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
                   Content = new StringContent(JsonConvert.SerializeObject(new
                   { })),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            var mockedHttpclient = new Mock<IHttpClientFactory>();
            mockedHttpclient.Setup(x => x.CreateClient("Spotify")).Returns(httpClient);

            var mockedMapper = new Mock<IMapper>();
            mockedMapper
           .Setup(mapper =>
            mapper.Map<SpotifyAuthorizationReadServiceModel>(It.IsAny<SpotifyAuthorizationResponseModel>()))
           .Returns(mockedReadServiceModel);

            var spotifyAutorizationServiceMock = new SpotifyAuthorizationService(
                new Lazy<ILogger<SpotifyAuthorizationService>>(mockedLogger.Object),
                mockedConfig.Object,
                new Lazy<IHttpClientFactory>(mockedHttpclient.Object),
                new Lazy<IMapper>(mockedMapper.Object));

            // Act
            var result = await spotifyAutorizationServiceMock.RefreshSpotifyTokens(It.IsAny<string>());

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ErrorType.FailedToRefreshSpotifyAccessToken.ToString());
        }
    }
}

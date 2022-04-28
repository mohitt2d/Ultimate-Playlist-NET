#region usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;
using UltimatePlaylist.Common.Mvc.Controllers;
using UltimatePlaylist.MobileApi.Models.Dsp;
using UltimatePlaylist.MobileApi.Models.Dsp.AppleMusic;
using UltimatePlaylist.MobileApi.Models.Dsp.Spotify;
using UltimatePlaylist.Services.Common.Interfaces.AppleMusic;
using UltimatePlaylist.Services.Common.Interfaces.Dsp;
using UltimatePlaylist.Services.Common.Interfaces.Spotify;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.Spotify;
using static UltimatePlaylist.Common.Mvc.Consts.Consts;

#endregion

namespace UltimatePlaylist.MobileApi.Area.Dsp
{
    [Area("DSP")]
    [Route("[controller]")]
    [AuthorizeRole(UserRole.User)]
    [ApiExplorerSettings(GroupName = MobileApiGroups.User)]
    public class DspController : BaseControllerWithAuthentication
    {
        #region Private Members

        private readonly Lazy<IMapper> MapperProvider;
        private readonly Lazy<ISpotifyService> SpotifyServiceProvider;
        private readonly Lazy<IDspService> DspServiceProvider;
        private readonly Lazy<IAppleMusicConnectionService> AppleMusicConnectionServiceProvider;
        private readonly Lazy<IAppleMusicSongService> AppleMusicSongServiceProvider;

        #endregion

        #region Constructor(s)

        public DspController(
            Lazy<IMapper> mapperProvider,
            Lazy<ISpotifyService> spotifyServiceProvider,
            Lazy<IDspService> dspServiceProvider,
            Lazy<IAppleMusicConnectionService> appleMusicConnectionServiceProvider,
            Lazy<IAppleMusicSongService> appleMusicSongServiceProvider)
        {
            MapperProvider = mapperProvider;
            SpotifyServiceProvider = spotifyServiceProvider;
            DspServiceProvider = dspServiceProvider;
            AppleMusicConnectionServiceProvider = appleMusicConnectionServiceProvider;
            AppleMusicSongServiceProvider = appleMusicSongServiceProvider;
        }

        #endregion

        #region Private Properites

        private IMapper Mapper => MapperProvider.Value;

        private ISpotifyService SpotifyService => SpotifyServiceProvider.Value;

        private IDspService DspService => DspServiceProvider.Value;

        private IAppleMusicConnectionService AppleMusicConnectionService => AppleMusicConnectionServiceProvider.Value;

        private IAppleMusicSongService AppleMusicSongService => AppleMusicSongServiceProvider.Value;

        #endregion

        #region GET

        [HttpGet("spotify-client-configuration")]
        [ProducesEnvelope(typeof(SpotifyClientConfigurationResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> SpotifyClientConfiguration()
        {
            return await SpotifyService.GetSpotifyClientConfigurationAsync(XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("user-dsp")]
        [ProducesEnvelope(typeof(IList<UserDspResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserDsps()
        {
            return await DspService.UserConnectedDsps(XUserExternalId)
               .Map(dps => Mapper.Map<IList<UserDspResponseModel>>(dps))
               .Finally(BuildEnvelopeResult);
        }

        [HttpGet("apple-music-developer-token")]
        [ProducesEnvelope(typeof(AppleMusicDeveloperTokenResponseModel), StatusCodes.Status200OK)]
        public IActionResult GetAppleMusicDeveloperToken()
        {
            return AppleMusicConnectionService.GetDeveloperTokenAsync()
                .Map(token => Mapper.Map<AppleMusicDeveloperTokenResponseModel>(token))
                .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region POST

        [HttpPost("apple-music-user-token")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAppleMusicUserToken([FromBody] AppleMusicUserTokenRequestModel requestModel)
        {
            return await AppleMusicConnectionService.SaveUserTokenAsync(requestModel.UserToken, XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("authorize-by-code")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthorizeSpotifyUserByCode([FromBody] SpotifyAuthorizationByCodeRequestModel spotifyAuthorizationRequestModel)
        {
            var writeServiceModel = Mapper.Map<SpotifyAuthorizationWriteServiceModel>(spotifyAuthorizationRequestModel);

            return await SpotifyService.AuthorizeByCode(XUserExternalId, writeServiceModel)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("authorize-with-tokens")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthorizeSpotifyUserWithTokens([FromBody] SpotifyAuthorizationWithTokensRequestModel spotifyAuthorizationWithTokensRequestModel)
        {
            var writeServiceModel = Mapper.Map<SpotifyAuthorizationWithTokensWriteServiceModel>(spotifyAuthorizationWithTokensRequestModel);

            return await SpotifyService.AuthorizeWithTokens(XUserExternalId, writeServiceModel)
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("add-song-to-spotify")]
        [ProducesEnvelope(typeof(AddSongToDspEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongToSpotify([FromBody] AddSongToSpotifyRequestModel requestModel)
        {
            var mapped = Mapper.Map<AddSongToSpotifyWriteServiceModel>(requestModel);

            return await SpotifyService.AddSongToUserSpotifyWithTicketsAsync(XUserExternalId, mapped)
               .Map(reqdServiceModel => Mapper.Map<AddSongToDspEarnedTicketsResponseModel>(reqdServiceModel))
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("add-song-to-apple-music")]
        [ProducesEnvelope(typeof(AddSongToDspEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongToAppleMusic([FromBody] AddSongToAppleMusicRequestModel requestModel)
        {
            var mapped = Mapper.Map<AddSongToAppleMusicWriteServiceModel>(requestModel);

            return await AppleMusicSongService.AddSongToAppleMusicWithTicketAsync(XUserExternalId, mapped)
               .Map(reqdServiceModel => Mapper.Map<AddSongToDspEarnedTicketsResponseModel>(reqdServiceModel))
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("history/add-song-to-spotify")]
        [ProducesEnvelope(typeof(AddSongToDspEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongToSpotifyWithoutPlaylistUpdate([FromBody] AddSongToSpotifyRequestModel requestModel)
        {
            var mapped = Mapper.Map<AddSongToSpotifyWriteServiceModel>(requestModel);

            return await SpotifyService.AddSongToUserSpotifyWithoutTicketsAsync(XUserExternalId, mapped)
               .Map(() => new AddSongToDspEarnedTicketsResponseModel() { LatestEarnedTickets = 0 })
               .Finally(BuildEnvelopeResult);
        }

        [HttpPost("history/add-song-to-apple-music")]
        [ProducesEnvelope(typeof(AddSongToDspEarnedTicketsResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSongToAppleMusicWithoutPlaylistUpdate([FromBody] AddSongToAppleMusicRequestModel requestModel)
        {
            var mapped = Mapper.Map<AddSongToAppleMusicWriteServiceModel>(requestModel);

            return await AppleMusicSongService.AddSongToAppleMusicWithoutTicketAsync(XUserExternalId, mapped)
               .Map(() => new AddSongToDspEarnedTicketsResponseModel() { LatestEarnedTickets = 0 })
               .Finally(BuildEnvelopeResult);
        }

        #endregion

        #region DELETE

        [HttpDelete("apple-music-user-token")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAppleMusicUserToken()
        {
            return await AppleMusicConnectionService.RemoveUserTokenAsync(DspType.AppleMusic, XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }

        [HttpDelete("spotify-remove-dsp")]
        [ProducesEmptyEnvelope(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveUserSpotifyDSP()
        {
            return await SpotifyService.RemoveUserSpotifyDSP(DspType.Spotify, XUserExternalId)
               .Finally(BuildEnvelopeResult);
        }
        #endregion
    }
}

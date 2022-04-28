#region Usings

using AutoMapper;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.MobileApi.Models.Dsp;
using UltimatePlaylist.MobileApi.Models.Dsp.AppleMusic;
using UltimatePlaylist.MobileApi.Models.Dsp.Spotify;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.AppleMusic;
using UltimatePlaylist.Services.Common.Models.Dsp;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Ticket;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class DspProfile : Profile
    {
        #region Constructor(s)

        public DspProfile()
        {
            CreateMap<SpotifyAuthorizationByCodeRequestModel, SpotifyAuthorizationWriteServiceModel>();

            CreateMap<AddSongToAppleMusicRequestModel, AddSongToAppleMusicWriteServiceModel>();

            CreateMap<AddSongToSpotifyRequestModel, AddSongToSpotifyWriteServiceModel>();

            CreateMap<SpotifyAuthorizationWithTokensRequestModel, SpotifyAuthorizationWithTokensWriteServiceModel>();

            CreateMap<UserDspReadServiceModel, UserDspResponseModel>()
                .IncludeBase<BaseReadServiceModel, BaseResponseModel>();

            CreateMap<DeveloperTokenReadServiceModel, AppleMusicDeveloperTokenResponseModel>();

            CreateMap<EarnedTicketsReadServiceModel, AddSongToDspEarnedTicketsResponseModel>();
        }

        #endregion

        #region Private Method(s)
        private Guid MapStringToGuid(string state)
        {
            Guid.TryParse(state, out var guid);

            return guid;
        }
        #endregion
    }
}

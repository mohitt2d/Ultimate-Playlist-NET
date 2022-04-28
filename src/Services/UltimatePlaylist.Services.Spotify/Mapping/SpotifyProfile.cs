#region Usings

using System;
using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Services.Common.Models.Dsp;
using UltimatePlaylist.Services.Common.Models.Spotify;
using UltimatePlaylist.Services.Common.Models.Spotify.Response;

#endregion

namespace UltimatePlaylist.Services.Spotify.Mapping
{
    public class SpotifyProfile : Profile
    {
        #region Constructor(s)

        public SpotifyProfile()
        {
            CreateMap<SpotifyAuthorizationResponseModel, SpotifyAuthorizationReadServiceModel>()
                .ForMember(p => p.AccessToken, opt => opt.MapFrom(p => p.Access_token))
                .ForMember(p => p.RefreshToken, opt => opt.MapFrom(p => p.Refresh_token))
                .ForMember(p => p.AccessTokenExpirationDate, opt => opt.MapFrom(p => GetAccessTokenExpirationDate(p.Expires_in)))
                .ForMember(p => p.TokenType, opt => opt.MapFrom(p => p.Token_type));

            CreateMap<UserDspEntity, UserDspReadServiceModel>();

            CreateMap<SpotifyAuthorizationWithTokensWriteServiceModel, SpotifyAuthorizationReadServiceModel>();
        }

        #endregion

        #region Private Method(s)

        public DateTime GetAccessTokenExpirationDate(int seconds)
        {
            return DateTime.UtcNow.AddSeconds(seconds);
        }
        #endregion
    }
}

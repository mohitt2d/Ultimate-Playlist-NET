#region Usings

using System.Web;
using AutoMapper;
using UltimatePlaylist.AdminApi.Models.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class IdentityProfile : Profile
    {
        #region Constructor(s)

        public IdentityProfile()
        {
            // Request model => Service write model
            CreateMap<ConfirmEmailRequestModel, ConfirmEmailWriteServiceModel>()
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => HttpUtility.UrlDecode(src.Token)));
            CreateMap<EmailChangedConfirmationRequestModel, EmailChangedConfirmationWriteServiceModel>()
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => HttpUtility.UrlDecode(src.Token)));
            CreateMap<ResetPasswordRequestModel, ResetPasswordWriteServiceModel>()
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => HttpUtility.UrlDecode(src.Token)));
            CreateMap<ChangePasswordRequestModel, ChangePasswordWriteServiceModel>();
            CreateMap<UserLoginRequestModel, UserLoginWriteServiceModel>();

            // Read service model => Response model
            CreateMap<AuthenticationReadServiceModel, AuthenticationResponseModel>();
        }

        #endregion
    }
}

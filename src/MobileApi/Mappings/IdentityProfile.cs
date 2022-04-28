#region Usings

using System.Web;
using AutoMapper;
using UltimatePlaylist.MobileApi.Models.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class IdentityProfile : Profile
    {
        #region Constructor(s)

        public IdentityProfile()
        {
            // Request model => Write service model
            CreateMap<UserRegistrationRequestModel, UserRegistrationWriteServiceModel>();
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

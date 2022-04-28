#region Usings

using AutoMapper;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.MobileApi.Models.UserProfile;
using UltimatePlaylist.Services.Common.Models;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Profile;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class UserProfile : Profile
    {
        #region Constructor(s)

        public UserProfile()
        {
            // Request model => Write service model
            CreateMap<UserPinRequestModel, UserPinWriteServiceModel>();

            CreateMap<UserProfileEditRequestModel, EditUserProfileWriteServiceModel>();

            // Read service model => Response model
            CreateMap<UserProfileInfoReadServiceModel, UserProfileInfoResponseModel>();

            CreateMap<FileReadServiceModel, UserAvatarFileResponseModel>()
                .ForMember(s => s.ExternalId, o => o.MapFrom(t => t.ExternalId))
                .ForMember(s => s.Url, o => o.MapFrom(t => t.Url));
        }

        #endregion
    }
}

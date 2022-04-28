#region Usings

using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Profile;

#endregion

namespace UltimatePlaylist.Services.Personalization.Mappings
{
    public class UserProfile : Profile
    {
        #region Constructor(s)

        public UserProfile()
        {
            // Entity model => Read service model
            CreateMap<AvatarFileEntity, FileReadServiceModel>();

            CreateMap<User, UserProfileInfoReadServiceModel>()
                .ForMember(s => s.FirstName, o => o.MapFrom(t => t.Name))
                .ForMember(s => s.Gender, o => o.MapFrom(t => t.Gender))
                .ForMember(s => s.Avatar, o => o.MapFrom(t => t.AvatarFile))
                .ForMember(s => s.IsPinEnabled, o => o.MapFrom(t => !string.IsNullOrEmpty(t.Pin)));
        }

        #endregion
    }
}
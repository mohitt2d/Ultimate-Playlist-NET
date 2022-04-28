#region Usings

using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Services.Common.Models.Identity;

#endregion

namespace UltimatePlaylist.Services.Identity.Mapping
{
    public class IdentityServiceProfile : Profile
    {
        public IdentityServiceProfile()
        {
            CreateMap<UserRegistrationWriteServiceModel, User>()
                .ForMember(p => p.UserName, opt => opt.MapFrom(p => p.Username))
                .ForMember(p => p.Name, opt => opt.MapFrom(p => p.FirstName))
                .ForMember(p => p.Gender, opt => opt.Ignore());
        }
    }
}

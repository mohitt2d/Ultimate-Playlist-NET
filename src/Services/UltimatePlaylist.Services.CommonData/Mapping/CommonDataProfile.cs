#region Usings

using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.Services.CommonData.Mapping
{
    public class CommonDataProfile : Profile
    {
        public CommonDataProfile()
        {
            // Entity => ReadServiceModel
            CreateMap<GenreEntity, SongGenresReadServiceModel>();
            CreateMap<GenderEntity, GenderReadServiceModel>();
        }
    }
}

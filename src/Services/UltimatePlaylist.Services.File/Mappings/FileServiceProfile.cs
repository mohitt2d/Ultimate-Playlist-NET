#region Usings

using AutoMapper;
using UltimatePlaylist.Database.Infrastructure.Entities.File;
using UltimatePlaylist.Services.Common.Models.Files;

#endregion

namespace UltimatePlaylist.Services.File.Mappings
{
    public class FileServiceProfile : Profile
    {
        public FileServiceProfile()
        {
            CreateMap<BaseFileEntity, FileReadServiceModel>();
            CreateMap<AudioFileEntity, AudioFileReadServiceModel>();
        }
    }
}

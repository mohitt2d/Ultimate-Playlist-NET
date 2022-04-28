#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UltimatePlaylist.AdminApi.Models.Media;
using UltimatePlaylist.Services.Common.Models.Files;
using UltimatePlaylist.Services.Common.Models.Media;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class MediaProfile : Profile
    {
        #region Constructor(s)

        public MediaProfile()
        {
            CreateMap<AudioAssetCreatedReadServiceModel, FileResponseModel>()
                .ForMember(m => m.ExternalId, opt => opt.MapFrom(m => m.FileExternalId));

            CreateMap<FileReadServiceModel, FileResponseModel>();
        }

        #endregion
    }
}

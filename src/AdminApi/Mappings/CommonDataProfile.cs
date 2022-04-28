#region Usings

using AutoMapper;
using UltimatePlaylist.AdminApi.Models.CommonData;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class CommonDataProfile : Profile
    {
        #region Constructor(s)

        public CommonDataProfile()
        {
            // Read service model => Response model
            CreateMap<SongGenresReadServiceModel, SongGenresResponseModel>();
        }

        #endregion
    }
}

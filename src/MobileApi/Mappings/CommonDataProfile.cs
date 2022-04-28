#region Usings

using AutoMapper;
using UltimatePlaylist.MobileApi.Models.CommonData;
using UltimatePlaylist.Services.Common.Models.CommonData;

#endregion

namespace UltimatePlaylist.MobileApi.Mappings
{
    public class CommonDataProfile : Profile
    {
        #region Constructor(s)

        public CommonDataProfile()
        {
            // Read service model => Response model
            CreateMap<GenderReadServiceModel, GenderResponseModel>();
        }

        #endregion
    }
}

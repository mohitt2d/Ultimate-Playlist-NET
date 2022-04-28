#region Usings

using AutoMapper;
using UltimatePlaylist.AdminApi.Mappings.TypeConverters;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.Services.Common.Models;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings
{
    public class CommonProfile : Profile
    {
        #region Constructor(s)

        public CommonProfile()
        {
            CreateMap<LookupServiceModel, LookupResponseModel>();
            CreateMap<GroupingReadServiceModel, GroupingResponseModel>();
            CreateMap(typeof(PaginatedReadServiceModel<>), typeof(PaginatedResponse<>))
                .ConvertUsing(typeof(PaginatedResponseTypeConverter<,>));
        }

        #endregion
    }
}

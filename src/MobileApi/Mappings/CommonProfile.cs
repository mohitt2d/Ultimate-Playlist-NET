#region Usings

using AutoMapper;
using UltimatePlaylist.Common.Mvc.Models;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.MobileApi.Mappings.TypeConverters;
using UltimatePlaylist.Services.Common.Models;

#endregion

namespace UltimatePlaylist.Api.Mappings
{
    public class CommonProfile : Profile
    {
        #region Constructor(s)

        public CommonProfile()
        {
            CreateMap<BaseReadServiceModel, BaseResponseModel>();
            CreateMap<LookupServiceModel, LookupResponseModel>();
            CreateMap<GroupingReadServiceModel, GroupingResponseModel>();
            CreateMap(typeof(PaginatedReadServiceModel<>), typeof(PaginatedResponse<>))
                .ConvertUsing(typeof(PaginatedResponseTypeConverter<,>));
        }

        #endregion
    }
}

#region Usings

using System.Collections.Generic;
using AutoMapper;
using UltimatePlaylist.Common.Mvc.Paging;
using UltimatePlaylist.Services.Common.Models;

#endregion

namespace UltimatePlaylist.AdminApi.Mappings.TypeConverters
{
    public class PaginatedResponseTypeConverter<TReadServiceModel, TResponseModel>
        : ITypeConverter<PaginatedReadServiceModel<TReadServiceModel>, PaginatedResponse<TResponseModel>>
        where TReadServiceModel : class
        where TResponseModel : class
    {
        public PaginatedResponse<TResponseModel> Convert(
            PaginatedReadServiceModel<TReadServiceModel> source,
            PaginatedResponse<TResponseModel> destination,
            ResolutionContext context)
        {
            var pagination = source.Pagination;
            var count = source.TotalCount;
            var items = context.Mapper.Map<IList<TResponseModel>>(source.Items);

            return new PaginatedResponse<TResponseModel>(items, count, pagination);
        }
    }
}

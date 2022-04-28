#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Models;

#endregion

namespace UltimatePlaylist.Services.Common.Models
{
    public class PaginatedReadServiceModel<T>
        : FilteredReadServiceModel<T>
        where T : class
    {
        public PaginatedReadServiceModel(
            IReadOnlyList<T> items,
            Pagination pagination,
            int totalCount)
            : base(items)
        {
            Pagination = pagination;
            TotalCount = totalCount;
        }

        public Pagination Pagination { get; }

        public int TotalCount { get; }
    }
}

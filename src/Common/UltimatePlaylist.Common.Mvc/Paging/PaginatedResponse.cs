#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Paging
{
    public class PaginatedResponse<TItems> : FilteredResponse<TItems>
        where TItems : class
    {
        #region Constructor(s)

        public PaginatedResponse(IList<TItems> items, int count, Pagination pagination, int? customCount = null)
            : base(items)
        {
            PageInfo = new PageInfo(count, pagination, customCount);
        }

        public PaginatedResponse()
        {
        }

        #endregion

        #region Public Properties

        public PageInfo PageInfo { get; set; }

        #endregion
    }
}
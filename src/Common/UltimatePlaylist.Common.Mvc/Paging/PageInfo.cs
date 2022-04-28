#region Usings

using System;
using UltimatePlaylist.Common.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Paging
{
    public class PageInfo
    {
        #region Constructor(s)

        public PageInfo(int count, Pagination pagination, int? customCount)
        {
            TotalCount = count;
            PageNumber = pagination.PageNumber ?? 0;
            PageSize = pagination.PageSize ?? 0;
            CustomCount = customCount;
            TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize ?? 0);
        }

        public PageInfo()
        {
        }

        #endregion

        #region Public Properties

        public int TotalCount { get; set; }

        public int? CustomCount { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        #endregion
    }
}

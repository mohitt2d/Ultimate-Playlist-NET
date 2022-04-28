#region Usings

#endregion

namespace UltimatePlaylist.Common.Models
{
    public class Pagination
    {
        #region Constructor(s)

        public Pagination(int pageSize, int pageNumber, string searchValue, string orderBy, bool desc)
        {
            PageSize = pageSize > 0 ? pageSize : (int?)null;
            PageNumber = pageNumber <= 0 ? (int?)null : pageNumber;
            SearchValue = searchValue.ToLower();
            OrderBy = orderBy;
            Descending = desc;
        }

        public Pagination(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber <= 1 ? 1 : pageNumber;
        }

        public Pagination(string searchValue)
        {
            PageSize = 100;
            PageNumber = 1;
            SearchValue = searchValue.ToLower();
            OrderBy = string.Empty;
            Descending = false;
        }

        public Pagination()
        {
            PageSize = 20;
            PageNumber = 1;
            SearchValue = string.Empty;
            OrderBy = string.Empty;
            Descending = false;
        }

        #endregion

        #region Public Properties

        public int? PageSize { get; }

        public int? PageNumber { get; }

        public string SearchValue { get; }

        public string OrderBy { get; }

        public bool Descending { get; }

        public int? Skip { get => (PageNumber - 1) * PageSize; }

        #endregion
    }
}

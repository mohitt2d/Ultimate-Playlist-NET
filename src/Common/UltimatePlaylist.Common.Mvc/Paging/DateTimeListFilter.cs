#region Usings

using System;
using UltimatePlaylist.Common.Extensions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Paging
{
    public class DateTimeListFilter
    {
        #region Constructor(s)

        public DateTimeListFilter(long? from, long? to)
        {
            From = from.FromUnixtimeStamp();
            To = to.FromUnixtimeStamp();
        }

        #endregion

        #region Public Properties

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        #endregion
    }
}

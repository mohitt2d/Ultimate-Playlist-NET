#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace UltimatePlaylist.Services.Common.Models
{
    public class FilteredReadServiceModel<T>
        where T : class
    {
        public FilteredReadServiceModel(IReadOnlyList<T> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IReadOnlyList<T> Items { get; }
    }
}

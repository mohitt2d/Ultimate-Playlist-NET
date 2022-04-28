#region Usings

using System;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace UltimatePlaylist.Common.Mvc.Utils
{
    public class LazyServiceProvider<T> : Lazy<T>
        where T : class
    {
        #region Constructor(s)

        public LazyServiceProvider(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }

        #endregion
    }
}
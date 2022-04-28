#region Usings

using System;
using System.Linq.Expressions;

#endregion

namespace UltimatePlaylist.Common.Filters.Eval.Interfaces
{
    public interface IFilterEval<in TFilter>
        where TFilter : FilterBase
    {
        Expression<Func<T, bool>> Eval<T>(TFilter filter);
    }
}

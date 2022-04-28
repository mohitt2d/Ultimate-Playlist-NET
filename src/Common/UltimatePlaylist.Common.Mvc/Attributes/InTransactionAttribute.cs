#region Usings

using Microsoft.AspNetCore.Mvc;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class InTransactionAttribute : ServiceFilterAttribute
    {
        public InTransactionAttribute()
            : base(typeof(EFTransactionAttribute))
        {
        }
    }
}

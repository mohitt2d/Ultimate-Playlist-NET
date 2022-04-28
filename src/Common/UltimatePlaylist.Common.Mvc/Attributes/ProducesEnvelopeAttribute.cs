#region Usings

using System;
using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class ProducesEnvelopeAttribute : ProducesResponseTypeAttribute
    {
        #region Constructor(s)

        public ProducesEnvelopeAttribute(Type objType, int statusCode)
            : base(statusCode)
        {
            Type = typeof(Envelope<>).MakeGenericType(objType);
        }

        #endregion
    }
}

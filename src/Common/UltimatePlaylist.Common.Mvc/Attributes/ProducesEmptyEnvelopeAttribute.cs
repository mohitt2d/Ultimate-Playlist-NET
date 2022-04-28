#region Usings

using Microsoft.AspNetCore.Mvc;
using UltimatePlaylist.Common.Mvc.Models;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class ProducesEmptyEnvelopeAttribute : ProducesResponseTypeAttribute
    {
        public ProducesEmptyEnvelopeAttribute(int statusCode)
           : base(typeof(EmptyEnvelope), statusCode)
        {
        }
    }
}

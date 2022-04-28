#region Usings

using System.Collections.Generic;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Common.Mvc.Exceptions;

#endregion

namespace UltimatePlaylist.Common.Mvc.Models
{
    public class EmptyEnvelope : Envelope<EmptyData>
    {
        public EmptyEnvelope()
            : base(new EmptyData())
        {
        }

        public EmptyEnvelope(Error error)
            : base(default, new List<Error> { error })
        {
        }

        public EmptyEnvelope(IList<Error> errors)
            : base(default, errors)
        {
        }
    }
}

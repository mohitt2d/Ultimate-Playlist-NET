#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UltimatePlaylist.Common.Enums;

#endregion

namespace UltimatePlaylist.Common.Mvc.Exceptions
{
    [Serializable]
    public class AppleMusicException : Exception
    {
        public AppleMusicException(string message)
       : base(message)
        {
        }

        public AppleMusicException(ErrorType errorType)
            : base(errorType.ToString())
        {
        }

        public AppleMusicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AppleMusicException(ErrorType error, Exception e)
            : base(error.ToString(), e)
        {
        }

        public AppleMusicException(ErrorType error, IEnumerable<Error> errors)
            : base(error.ToString())
        {
            Errors = errors.ToList();
        }

        protected AppleMusicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<Error> Errors { get; set; }
    }
}

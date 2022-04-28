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
    public class LoginException : Exception
    {
        #region Constructor(s)

        public LoginException(string message)
            : base(message)
        {
        }

        public LoginException(ErrorType errorType)
            : base(errorType.ToString())
        {
        }

        public LoginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public LoginException(ErrorType error, Exception e)
            : base(error.ToString(), e)
        {
        }

        public LoginException(ErrorType error, IEnumerable<Error> errors)
            : base(error.ToString())
        {
            Errors = errors.ToList();
        }

        protected LoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<Error> Errors { get; set; }

        #endregion
    }
}
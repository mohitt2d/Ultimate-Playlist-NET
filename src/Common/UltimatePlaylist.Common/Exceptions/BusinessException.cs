#region Usings

using System;
using System.Runtime.Serialization;

#endregion

namespace UltimatePlaylist.Common.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        #region Constructor(s)

        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private BusinessException()
        {
        }

        #endregion
    }
}
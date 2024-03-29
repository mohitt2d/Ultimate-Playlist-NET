﻿#region Usings

using System;
using System.Runtime.Serialization;

#endregion

namespace UltimatePlaylist.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        #region Constructor(s)

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private NotFoundException()
        {
        }

        #endregion
    }
}
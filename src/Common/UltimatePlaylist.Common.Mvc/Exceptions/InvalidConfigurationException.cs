#region Usings

using System;

#endregion

namespace UltimatePlaylist.Common.Mvc.Exceptions
{
    public class InvalidConfigurationException : Exception
    {
        #region Constructor(s)

        public InvalidConfigurationException(string message)
            : base(message)
        {
        }

        #endregion
    }
}

#region Usings

using System.Runtime.Serialization;

#endregion

namespace UltimatePlaylist.Services.Games.Exceptions
{
    [Serializable]
    public class UlttimatePayoutGameJobException : Exception
    {
        #region Constructor(s)

        public UlttimatePayoutGameJobException(string message)
            : base(message)
        {
        }

        public UlttimatePayoutGameJobException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UlttimatePayoutGameJobException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private UlttimatePayoutGameJobException()
        {
        }

        #endregion
    }
}

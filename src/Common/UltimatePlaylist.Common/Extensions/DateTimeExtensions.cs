#region Using

using System;

#endregion

namespace UltimatePlaylist.Common.Extensions
{
    public static class DateTimeExtensions
    {
        #region Public Methods

        #region Unix Timestamp

        public static long ToUnixTimestamp(this DateTime date)
        {
            return (long)Math.Floor((date.ToUniversalTimeWhenUnspecified() - DateTimeOffset.UnixEpoch).TotalSeconds);
        }

        public static long? ToUnixTimestamp(this DateTime? date)
        {
            if (date.HasValue)
            {
                return (long)Math.Floor((date.Value.ToUniversalTimeWhenUnspecified() - DateTimeOffset.UnixEpoch).TotalSeconds);
            }

            return null;
        }

        public static DateTime FromUnixTimestamp(this long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }

        public static bool TryFromStringUnixTimestamp(this string unixTimeStampString, out DateTime? dateTime)
        {
            try
            {
                if (string.IsNullOrEmpty(unixTimeStampString))
                {
                    dateTime = null;
                    return true;
                }

                dateTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(unixTimeStampString)).DateTime;
                return true;
            }
            catch
            {
                dateTime = null;
                return false;
            }
        }

        public static DateTime? FromUnixtimeStamp(this long? unixTimeStamp)
        {
            if (unixTimeStamp.HasValue)
            {
                return unixTimeStamp.Value.FromUnixTimestamp();
            }

            return null;
        }

        #endregion

        #region ToUniversalTimeWhenUnspecified

        public static DateTime ToUniversalTimeWhenUnspecified(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }

            return date.ToUniversalTime();
        }

        #endregion

        #region Truncate

        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
        }

        #endregion

        #endregion
    }
}
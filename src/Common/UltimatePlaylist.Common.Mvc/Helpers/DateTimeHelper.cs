namespace UltimatePlaylist.Common.Mvc.Helpers
{
    public static class DateTimeHelper
    {
        #region ToUTCTimeForTimeZoneRelative

        public static DateTime ToTodayUTCTimeForTimeZoneRelativeTime(string timezoneId)
        {
            TimeZoneInfo targetTimezone = TimeZoneInfo.Utc;
            DateTime dateTime = DateTime.UtcNow;
            
            try
            {
                targetTimezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, timezoneId);
            }
            catch (TimeZoneNotFoundException)
            {
            }

            var dateTimeUnspec = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Unspecified);
            var temp = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, targetTimezone);
            if (timezoneId == "US Eastern Standard Time")
            {
                temp = temp.AddHours(1);
            }
            return temp;
        }

        #endregion

        #region ToUTCTimeForTimeZoneRelativeTime

        public static DateTime ToUTCTimeForTimeZoneRelativeTime(DateTime dateTime, string timezoneId)
        {
            TimeZoneInfo targetTimezone = TimeZoneInfo.Utc;

            try
            {
                targetTimezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, timezoneId);
            }
            catch (TimeZoneNotFoundException)
            {
            }

            var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, targetTimezone);
        }

        #endregion
    }
}
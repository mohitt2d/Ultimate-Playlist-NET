#region Usings

using Hangfire;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Notification.Jobs;

#endregion

namespace UltimatePlaylist.Services.Notification
{
    public static class NotificationReminderScheduler
    {
        public static void ScheduleForReminderNotifications(
            IRecurringJobManager recurringJobManager,
            NotificationConfig notificationConfig,
            GamesConfig gamesConfig,
            PlaylistConfig playlistConfig)
        {
            /*if (gamesConfig.TestMode)
            {
                recurringJobManager.AddOrUpdate<NotificationReminderJob>(
                  nameof(NotificationReminderJob),
                  p => p.RunReminderNotifications(),
                  Cron.HourInterval(1),
                  timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
            else
            {
                recurringJobManager.AddOrUpdate<NotificationReminderJob>(
                   nameof(NotificationReminderJob),
                   p => p.RunReminderNotifications(),
                   Cron.Daily(notificationConfig.Reminder.Hour, notificationConfig.Reminder.Minutes),
                   timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }*/
        }

        public static void RemoveReminderNotificationJobs(IRecurringJobManager recurringJobManager)
        {
            // recurringJobManager.RemoveIfExists(nameof(NotificationReminderJob));
        }
    }
}

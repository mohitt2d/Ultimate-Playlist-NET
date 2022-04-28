#region Usings

using Hangfire;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Notification.Jobs;

#endregion

namespace UltimatePlaylist.Services.Notification
{
    public static class NotificationBeforeGamesScheduler
    {
        public static void ScheduleForNotificationsBeforeGames(
            IRecurringJobManager recurringJobManager,
            NotificationConfig notificationConfig,
            GamesConfig gamesConfig,
            PlaylistConfig playlistConfig)
        {
            if (gamesConfig.TestMode)
            {
                recurringJobManager.AddOrUpdate<NotificationBeforeGamesJob>(
                  nameof(NotificationBeforeGamesJob),
                  p => p.RunNotificationsBeforeGame(),
                  Cron.Hourly(notificationConfig.BeforeGames.Minutes),
                  timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
            else
            {
                recurringJobManager.AddOrUpdate<NotificationBeforeGamesJob>(
                   nameof(NotificationBeforeGamesJob),
                   p => p.RunNotificationsBeforeGame(),
                   Cron.Daily(notificationConfig.BeforeGames.Hour, notificationConfig.BeforeGames.Minutes),
                   timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
        }

        public static void RemoveNotificationBeforeGamesJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.RemoveIfExists(nameof(NotificationBeforeGamesJob));
        }
    }
}

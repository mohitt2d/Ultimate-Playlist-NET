#region Usings

using Hangfire;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Notification.Jobs;

#endregion

namespace UltimatePlaylist.Services.Notification
{
    public static class NotificationAfterGamesScheduler
    {
        public static void ScheduleForNotificationsAfterGames(
            IRecurringJobManager recurringJobManager,
            NotificationConfig notificationConfig,
            GamesConfig gamesConfig,
            PlaylistConfig playlistConfig)
        {
            if (gamesConfig.TestMode)
            {
                recurringJobManager.AddOrUpdate<NotificationAfterGamesJob>(
                  nameof(NotificationAfterGamesJob),
                  p => p.RunNotificationsAfterGame(),
                  Cron.MinuteInterval(5),
                  timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
            else
            {
                recurringJobManager.AddOrUpdate<NotificationAfterGamesJob>(
                   nameof(NotificationAfterGamesJob),
                   p => p.RunNotificationsAfterGame(),
                   Cron.Daily(notificationConfig.AfterGames.Hour, notificationConfig.AfterGames.Minutes),
                   timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
        }

        public static void RemoveNotificationAfterGamesJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.RemoveIfExists(nameof(NotificationAfterGamesJob));
        }
    }
}

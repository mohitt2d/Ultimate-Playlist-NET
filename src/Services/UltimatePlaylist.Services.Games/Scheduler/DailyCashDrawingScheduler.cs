#region Usings

using Hangfire;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Services.Games.Jobs;

#endregion

namespace UltimatePlaylist.Services.Games
{
    public static class DailyCashDrawingScheduler
    {
        public static void SchedulealiCashDrawingJob(IRecurringJobManager recurringJobManager, GamesConfig gamesConfig, PlaylistConfig playlistConfig)
        {
            // TODO: For testing purposes game will run hourly
            if (gamesConfig.TestMode)
            {
                recurringJobManager.AddOrUpdate<DailyCashGameJob>(
                  nameof(DailyCashGameJob),
                  p => p.RunDailyCashGame(),
                  Cron.Hourly(),
                  timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
            else
            {
                recurringJobManager.AddOrUpdate<DailyCashGameJob>(
                   nameof(DailyCashGameJob),
                   p => p.RunDailyCashGame(),
                   Cron.Daily(playlistConfig.StartDateOffSet.Hours, playlistConfig.StartDateOffSet.Minutes),
                   timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
        }

        public static void RemoveDaliCashDrawingJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.RemoveIfExists(nameof(DailyCashGameJob));
        }
    }
}

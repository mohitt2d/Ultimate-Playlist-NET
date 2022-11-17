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
            playlistConfig.TimeZone = "US Eastern Standard Time";
            // The timeZone is (UTC-05:00) Indiana (East) when playlistConfig.TimeZone is US Eastern Standard Time.
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
                   Cron.Monthly(),
                   timeZone: TimeZoneInfo.FindSystemTimeZoneById(playlistConfig.TimeZone));
            }
        }

        public static void RemoveDaliCashDrawingJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.RemoveIfExists(nameof(DailyCashGameJob));
        }
    }
}

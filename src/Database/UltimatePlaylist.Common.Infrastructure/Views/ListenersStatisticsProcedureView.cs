namespace UltimatePlaylist.Database.Infrastructure.Views
{
    public class ListenersStatisticsProcedureView
    {
        public int TotalListeners { get; set; }

        public double AverageDailyPlaysPerUser { get; set; }

        public double AverageTimeListenedUser { get; set; }

        public int TotalDailyListeners { get; set; }

        public int TotalMaxListeners { get; set; }

        public double TotalAverageListeners { get; set; }
    }
}

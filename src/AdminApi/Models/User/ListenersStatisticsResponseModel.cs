namespace UltimatePlaylist.AdminApi.Models.User
{
    public class ListenersStatisticsResponseModel
    {
        public int TotalListeners { get; set; }

        public decimal AverageDailyPlaysPerUser { get; set; }

        public TimeSpan AverageTimeListenedUser { get; set; }

        public int TotalDailyListeners { get; set; }

        public int TotalMaxListeners { get; set; }

        public decimal TotalAverageListeners { get; set; }
    }
}

namespace UltimatePlaylist.Common.Config
{
    public class NotificationConfig
    {
        public NotificationCronConfig BeforeGames { get; set; }

        public NotificationCronConfig AfterGames { get; set; }

        public NotificationCronConfig Reminder { get; set; }

        public string SupportEmailAddress { get; set; }
    }
}

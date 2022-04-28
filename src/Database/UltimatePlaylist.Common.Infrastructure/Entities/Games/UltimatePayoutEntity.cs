namespace UltimatePlaylist.Database.Infrastructure.Entities.Games
{
    public class UltimatePayoutEntity : GameBaseEntity
    {
        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public int ThirdNumber { get; set; }

        public int FourthNumber { get; set; }

        public int FifthNumber { get; set; }

        public int SixthNumber { get; set; }

        public decimal Reward { get; set; }

        public virtual ICollection<UserLotteryEntryEntity> UserLotteryEntries { get; set; }
    }
}

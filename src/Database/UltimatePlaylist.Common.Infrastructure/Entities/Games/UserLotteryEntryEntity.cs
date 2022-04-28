#region Usings

using UltimatePlaylist.Database.Infrastructure.Entities.Base;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games
{
    public class UserLotteryEntryEntity : BaseEntity
    {
        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public int ThirdNumber { get; set; }

        public int FourthNumber { get; set; }

        public int FifthNumber { get; set; }

        public int SixthNumber { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public long GameId { get; set; }

        public virtual UltimatePayoutEntity Game { get; set; }
    }
}

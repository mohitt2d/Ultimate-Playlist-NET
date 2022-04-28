#region Usings

using System;

#endregion

namespace UltimatePlaylist.Games.Models.Lottery
{
    public class LotteryWinningNumbersReadServiceModel
    {
        public Guid LotteryId { get; set; }

        public DateTime DateTime { get; set; }

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public int ThirdNumber { get; set; }

        public int FourthNumber { get; set; }

        public int FifthNumber { get; set; }

        public int SixthNumber { get; set; }
    }
}

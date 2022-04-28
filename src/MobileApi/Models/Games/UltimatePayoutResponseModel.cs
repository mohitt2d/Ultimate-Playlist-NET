﻿namespace UltimatePlaylist.MobileApi.Models.Games
{
    public class UltimatePayoutResponseModel
    {
        public string NextUltimatePrize { get; set; }

        public DateTime NextUltimateDate { get; set; }

        public int TicketsCount { get; set; }

        public UltimatePayoutWinnerResponseModel UltimatePayoutWinner { get; set; }

        public int[] UltimatePayoutWinningNumbers { get; set; }

        public IList<int[]> UltimatePayoutUserNumbers { get; set; }
    }
}

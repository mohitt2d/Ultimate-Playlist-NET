using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Entities.Games;

namespace UltimatePlaylist.Services.Common.Models.Games
{
    public class JackpotWinnersAndNumbersResponseModel
    {
        public DateTime GameDate { get; set; }
        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public int ThirdNumber { get; set; }

        public int FourthNumber { get; set; }

        public int FifthNumber { get; set; }

        public int SixthNumber { get; set; }

        public decimal Reward { get; set; }

        public string WinnerFullName { get; set; }
        public string WinnerUsername { get; set; }
        public string WinnerAvatarUrl { get; set; }

    }
}
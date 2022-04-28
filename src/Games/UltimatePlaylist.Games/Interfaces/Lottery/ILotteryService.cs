#region Usings

using CSharpFunctionalExtensions;
using UltimatePlaylist.Games.Models.Lottery;

#endregion

namespace UltimatePlaylist.Games.Interfaces
{
    public interface ILotteryService
    {
        public Result<LotteryWinningNumbersReadServiceModel> GetLotteryWinningNumbers();
    }
}

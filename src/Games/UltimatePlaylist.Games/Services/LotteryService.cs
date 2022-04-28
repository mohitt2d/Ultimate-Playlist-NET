#region Usings

using CSharpFunctionalExtensions;
using System;
using System.Security.Cryptography;
using UltimatePlaylist.Games.Const;
using UltimatePlaylist.Games.Interfaces;
using UltimatePlaylist.Games.Models.Lottery;

#endregion

namespace UltimatePlaylist.Games.Services
{
    public class LotteryService : ILotteryService
    {
        public Result<LotteryWinningNumbersReadServiceModel> GetLotteryWinningNumbers() =>  Result.Success(new LotteryWinningNumbersReadServiceModel()
        {
                LotteryId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                FirstNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                SecondNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                ThirdNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                FourthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                FifthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.OneToFiveNumbersRangeExclusive),
                SixthNumber = RandomNumberGenerator.GetInt32(1, LotteryRanges.SixthNumberRangeExclusive),
        });

    }
}

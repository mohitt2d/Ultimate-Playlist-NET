#region Usings

using FluentAssertions;
using UltimatePlaylist.Games.Const;
using UltimatePlaylist.Games.Services;
using Xunit;

#endregion

namespace UltmiatePlaylist.Games.Tests.Services
{
    public class LotteryServiceTests
    {
        [Fact]
        public void GetLotteryWinningSucess()
        {
            var lotteryService = new LotteryService();

            var result = lotteryService.GetLotteryWinningNumbers();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.FirstNumber.Should().BeInRange(1, LotteryRanges.OneToFiveNumbersRangeExclusive);
            result.Value.SecondNumber.Should().BeInRange(1, LotteryRanges.OneToFiveNumbersRangeExclusive);
            result.Value.ThirdNumber.Should().BeInRange(1, LotteryRanges.OneToFiveNumbersRangeExclusive);
            result.Value.FourthNumber.Should().BeInRange(1, LotteryRanges.OneToFiveNumbersRangeExclusive);
            result.Value.FifthNumber.Should().BeInRange(1, LotteryRanges.OneToFiveNumbersRangeExclusive);

            result.Value.SixthNumber.Should().BeInRange(1, LotteryRanges.SixthNumberRangeExclusive);
        }
    }
}

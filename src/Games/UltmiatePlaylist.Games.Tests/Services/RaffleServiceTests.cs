#region Usings

using FluentAssertions;
using UltimatePlaylist.Games.Services;
using UltmiatePlaylist.Games.Tests.Helpers;
using Xunit;

#endregion

namespace UltmiatePlaylist.Games.Tests.Services
{
    public class RaffleServiceTests
    {
        private const long TicketsQuantity = 10000000;
        private const int Selection = 50;

        [Fact]
        public void GetRaffleWinnerSuccess()
        {
            var raffleTickets = RaffleTicketsHelper.GenerateRaffleTickets(TicketsQuantity);
            var raffleService = new RaffleService();
            
            var result = raffleService.GetRaffleWinners(raffleTickets, Selection);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();

            if(Selection <= TicketsQuantity)
            {
                result.Value.Should().HaveCount(Selection);
            }

            raffleTickets.Should().Contain(result.Value);
        }
    }
}

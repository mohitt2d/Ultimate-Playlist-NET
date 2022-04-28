#region Usings

using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UltimatePlaylist.Games.Const;
using UltimatePlaylist.Games.Interfaces;
using UltimatePlaylist.Games.Models.Raffle;

#endregion

namespace UltimatePlaylist.Games.Services
{
    public class RaffleService : IRaffleService
    {
        public Result<IEnumerable<RaffleUserTicketReadServiceModel>> GetRaffleWinners(ICollection<RaffleUserTicketReadServiceModel> raffleUsers, int selections)
        {
            return Result.SuccessIf(raffleUsers.Count != 0 && selections > 0, ErrorMessages.RaffleTitcketsCollectionIsEmpty)
                .Map(() => Shuffle(raffleUsers))
                .Map(shuffleCollection => shuffleCollection.Take(selections));
        }

        private IEnumerable<RaffleUserTicketReadServiceModel> Shuffle(ICollection<RaffleUserTicketReadServiceModel> source)
        {
            var elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = RandomNumberGenerator.GetInt32(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}

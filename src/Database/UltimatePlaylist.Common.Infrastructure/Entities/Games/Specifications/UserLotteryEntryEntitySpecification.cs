#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications
{
    public class UserLotteryEntryEntitySpecification : BaseSpecification<UserLotteryEntryEntity>
    {
        #region Constructor(s)

        public UserLotteryEntryEntitySpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UserLotteryEntryEntitySpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public UserLotteryEntryEntitySpecification ByGameId(long gameId)
        {
            AddCriteria(t => t.GameId == gameId);

            return this;
        }

        public UserLotteryEntryEntitySpecification ByUserId(Guid userExternalId)
        {
            AddCriteria(t => t.User.ExternalId == userExternalId);

            return this;
        }

        public UserLotteryEntryEntitySpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        public UserLotteryEntryEntitySpecification ByGameNumbers(int first, int second, int third, int fourth, int fifth, int sixth)
        {
            AddCriteria(t => t.FirstNumber.Equals(first));
            AddCriteria(t => t.SecondNumber.Equals(second));
            AddCriteria(t => t.ThirdNumber.Equals(third));
            AddCriteria(t => t.FourthNumber.Equals(fourth));
            AddCriteria(t => t.FifthNumber.Equals(fifth));
            AddCriteria(t => t.SixthNumber.Equals(sixth));

            return this;
        }

        #endregion

        #region Includes

        public UserLotteryEntryEntitySpecification WithUser()
        {
            AddInclude(c => c.User);

            return this;
        }

        public UserLotteryEntryEntitySpecification WithGame()
        {
            AddInclude(c => c.Game);

            return this;
        }

        #endregion
    }
}

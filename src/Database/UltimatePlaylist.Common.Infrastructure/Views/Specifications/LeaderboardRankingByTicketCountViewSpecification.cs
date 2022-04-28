#region Usings

using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion
namespace UltimatePlaylist.Database.Infrastructure.Views.Specifications
{
    public class LeaderboardRankingByTicketCountViewSpecification : BaseSpecification<LeaderboardRankingByTicketCountView>
    {
        #region Constructor(s)

        public LeaderboardRankingByTicketCountViewSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public LeaderboardRankingByTicketCountViewSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(c => c.ExternalId == userExternalId);

            return this;
        }

        #endregion

        #region Include

        #endregion

        #region Pagination

        public LeaderboardRankingByTicketCountViewSpecification Paged(Pagination pagination = null)
        {
            ApplyOrderBy(c => c.TicketCount, true);
            ApplyPaging(pagination);

            return this;
        }

        #endregion
    }
}

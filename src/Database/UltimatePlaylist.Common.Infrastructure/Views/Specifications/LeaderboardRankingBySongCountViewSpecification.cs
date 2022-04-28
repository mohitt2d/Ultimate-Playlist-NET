#region Usings

using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Views.Specifications
{
    public class LeaderboardRankingBySongCountViewSpecification : BaseSpecification<LeaderboardRankingBySongCountView>
    {
        #region Constructor(s)

        public LeaderboardRankingBySongCountViewSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public LeaderboardRankingBySongCountViewSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(c => c.ExternalId == userExternalId);

            return this;
        }

        #endregion

        #region Include

        #endregion

        #region Pagination

        public LeaderboardRankingBySongCountViewSpecification Paged(Pagination pagination = null)
        {
            ApplyOrderBy(c => c.SongCount, true);
            ApplyPaging(pagination);

            return this;
        }

        #endregion
    }
}

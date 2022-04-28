#region Usings

using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Views.Specifications
{
    public class SongPopularityViewSpecification : BaseSpecification<SongPopularityView>
    {
        #region Constructor(s)

        public SongPopularityViewSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public SongPopularityViewSpecification ByExternalIds(IList<Guid> guids)
        {
            AddCriteria(c => guids.Contains(c.ExternalId));

            return this;
        }

        #endregion
    }
}

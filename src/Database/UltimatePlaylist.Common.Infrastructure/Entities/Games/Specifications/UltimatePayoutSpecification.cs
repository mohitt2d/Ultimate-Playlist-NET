#region Usings

using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications
{
    public class UltimatePayoutSpecification : BaseSpecification<UltimatePayoutEntity>
    {
        #region Constructor(s)

        public UltimatePayoutSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UltimatePayoutSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public UltimatePayoutSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        public UltimatePayoutSpecification ByGameDate(DateTime dateTime)
        {
            AddCriteria(t => t.GameDate.Date.Equals(dateTime.Date));

            return this;
        }

        public UltimatePayoutSpecification ByGameDateRange(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            AddCriteria(t => dateTimeFrom.Date <= t.GameDate.Date && t.GameDate.Date <= dateTimeTo.Date);

            return this;
        }

        public UltimatePayoutSpecification ByIsFinished(bool isFinished)
        {
            AddCriteria(t => t.IsFinished == isFinished);

            return this;
        }

        #endregion

        #region Includes

        public UltimatePayoutSpecification WithWinners()
        {
            AddInclude(t => t.Winnings);
            AddInclude(t => t.Winnings.Select(w => w.Winner));
            AddInclude(t => t.Winnings.Select(w => w.Winner.AvatarFile));

            return this;
        }

        #endregion

        #region OrderBy

        public UltimatePayoutSpecification OrderByCreated(bool desc)
        {
            ApplyOrderBy(c => c.Created, desc);

            return this;
        }

        #endregion
    }
}

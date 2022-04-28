#region Usings

using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications
{
    public class DailyCashDrawingSpecification : BaseSpecification<DailyCashDrawingEntity>
    {
        #region Constructor(s)

        public DailyCashDrawingSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public DailyCashDrawingSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public DailyCashDrawingSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        #endregion

        #region OrderBy

        public DailyCashDrawingSpecification OrderByCreated(bool desc)
        {
            ApplyOrderBy(c => c.Created, desc);

            return this;
        }

        #endregion
    }
}
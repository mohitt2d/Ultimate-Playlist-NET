#region Usings

using System;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Identity.Specifications
{
    public class GenderSpecification : BaseSpecification<GenderEntity>
    {
        #region Constructor(s)

        public GenderSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public GenderSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(s => s.ExternalId == externalId);

            return this;
        }

        #endregion
    }
}

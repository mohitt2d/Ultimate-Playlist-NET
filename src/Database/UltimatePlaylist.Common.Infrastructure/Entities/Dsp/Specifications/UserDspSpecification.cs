#region Usings

using System;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Dsp.Specifications
{
    public class UserDspSpecification : BaseSpecification<UserDspEntity>
    {
        #region Constructor(s)

        public UserDspSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public UserDspSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(s => s.User.ExternalId == userExternalId);

            return this;
        }

        public UserDspSpecification ByType(DspType type)
        {
            AddCriteria(s => s.Type == type);

            return this;
        }

        public UserDspSpecification ByActive()
        {
            AddCriteria(s => s.IsActive);

            return this;
        }

        #endregion

        #region Ordering

        public UserDspSpecification OrderByCreatedDescending()
        {
            ApplyOrderByDescending(s => s.Created);

            return this;
        }

        #endregion

        #region Include

        public UserDspSpecification WithUser()
        {
            AddInclude(s => s.User);

            return this;
        }

        #endregion
    }
}

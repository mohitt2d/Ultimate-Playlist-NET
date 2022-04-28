#region Usings

using System;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Specifications
{
    public class AvatarFileSpecification : BaseSpecification<AvatarFileEntity>
    {
        #region Constructor(s)

        public AvatarFileSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public AvatarFileSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public AvatarFileSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        #endregion
    }
}

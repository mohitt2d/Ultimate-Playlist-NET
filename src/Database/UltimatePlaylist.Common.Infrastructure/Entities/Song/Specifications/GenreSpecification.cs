#region Usings

using System;
using System.Collections.Generic;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Specifications
{
    public class GenreSpecification : BaseSpecification<GenreEntity>
    {
        #region Constructor(s)

        public GenreSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public GenreSpecification ByExternalIds(IList<Guid> externalIds)
        {
            AddCriteria(c => externalIds.Contains(c.ExternalId));

            return this;
        }

        #endregion

        #region Include

        #endregion
    }
}

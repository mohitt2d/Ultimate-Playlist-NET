#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Specifications
{
    public class CoverFileSpecification : BaseSpecification<CoverFileEntity>
    {
        #region Constructor(s)

        public CoverFileSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public CoverFileSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public CoverFileSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        public CoverFileSpecification ByFileName(string fileName)
        {
            AddCriteria(t => t.FileName == fileName);

            return this;
        }

        #endregion
    }
}

#region Usings

using System;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.File.Specifications
{
    public class AudioFileSpecification : BaseSpecification<AudioFileEntity>
    {
        #region Constructor(s)

        public AudioFileSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public AudioFileSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public AudioFileSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        public AudioFileSpecification ByJobName(string jobName)
        {
            AddCriteria(t => t.JobName == jobName);

            return this;
        }

        public AudioFileSpecification ByFileName(string fileName)
        {
            AddCriteria(t => t.FileName == fileName);

            return this;
        }

        #endregion
    }
}

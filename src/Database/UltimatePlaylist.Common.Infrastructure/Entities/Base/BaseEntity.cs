#region Usings

using System;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        #region Public properties

        public long Id { get; set; }

        public Guid ExternalId { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsDeleted { get; set; }

        #endregion

        #region Public method(s)

        public virtual void MarkDeleted()
        {
            IsDeleted = true;
        }

        #endregion
    }
}
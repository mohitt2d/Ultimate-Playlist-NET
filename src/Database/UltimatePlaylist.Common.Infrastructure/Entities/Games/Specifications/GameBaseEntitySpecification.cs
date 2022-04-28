#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications
{
    public class GameBaseEntitySpecification : BaseSpecification<GameBaseEntity>
    {
        #region Constructor(s)

        public GameBaseEntitySpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public GameBaseEntitySpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public GameBaseEntitySpecification ByFinishedStatus(bool finished)
        {
            AddCriteria(t => t.IsFinished == finished);

            return this;
        }

        public GameBaseEntitySpecification ByType(GameType type)
        {
            AddCriteria(t => t.Type == type);

            return this;
        }

        public GameBaseEntitySpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        #endregion

        #region OrderBy

        public GameBaseEntitySpecification OrderByCreated(bool desc)
        {
            ApplyOrderBy(c => c.Created, desc);

            return this;
        }

        #endregion
    }
}

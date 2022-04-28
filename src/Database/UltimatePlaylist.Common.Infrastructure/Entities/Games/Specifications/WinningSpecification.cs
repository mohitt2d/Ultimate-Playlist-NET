#region Usings

using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Common.Filters.Models;
using UltimatePlaylist.Common.Models;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Games.Specifications
{
    public class WinningSpecification : BaseSpecification<WinningEntity>
    {
        #region Constructor(s)

        public WinningSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(c => !c.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public WinningSpecification ById(long id)
        {
            AddCriteria(t => t.Id == id);

            return this;
        }

        public WinningSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(t => t.ExternalId.Equals(externalId));

            return this;
        }

        public WinningSpecification ByUserExternalId(Guid userExternalId)
        {
            AddCriteria(t => t.Winner.ExternalId.Equals(userExternalId));

            return this;
        }

        public WinningSpecification ByGameId(long gameId)
        {
            AddCriteria(t => t.GameId == gameId);

            return this;
        }

        public WinningSpecification ByStatus(WinningStatus status)
        {
            AddCriteria(t => t.Status == status);

            return this;
        }

        public WinningSpecification ByNotCollectedStatus()
        {
            AddCriteria(t => t.Status != WinningStatus.Paid && t.Status != WinningStatus.Rejected);

            return this;
        }

        #endregion

        #region Includes

        public WinningSpecification WithUser()
        {
            AddInclude(t => t.Winner);
            AddInclude(t => t.Winner.AvatarFile);

            return this;
        }

        public WinningSpecification WithGame()
        {
            AddInclude(t => t.Game);

            return this;
        }

        #endregion

        #region Pagination

        public WinningSpecification Pagination(Pagination pagination = null)
        {
            if (pagination == null)
            {
                ApplyOrderBy(c => c.Created, true);
            }
            else
            {
                ApplyOrderBy(pagination);
                ApplyPaging(pagination);
            }

            return this;
        }

        public WinningSpecification Filter(IEnumerable<FilterModel> filter)
        {
            ApplyFilters(filter);

            return this;
        }

        public WinningSpecification Search(string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                AddCriteria(x => x.Winner.UserName.Contains(searchValue)
                    || x.Winner.Name.Contains(searchValue)
                    || x.Winner.LastName.Contains(searchValue)
                    || x.Winner.PhoneNumber.Contains(searchValue)
                    || x.Winner.Email.Contains(searchValue));
            }

            return this;
        }

        public WinningSpecification Filter(
            WinningStatus? winningStatus,
            GameType? gameType,
            int? minAge,
            int? maxAge,
            bool? isAgeVerified)
        {
            if (winningStatus.HasValue)
            {
                AddCriteria(x => x.Status == winningStatus);
            }

            if (gameType.HasValue)
            {
                AddCriteria(x => x.Game.Type == gameType);
            }

            if (isAgeVerified.HasValue)
            {
                AddCriteria(x => x.Winner.IsAgeVerified == isAgeVerified);
            }

            if (minAge.HasValue || maxAge.HasValue)
            {
                var maxBirthDate = minAge.HasValue ? DateTime.UtcNow.AddYears(-minAge.Value) : DateTime.UtcNow;
                var minBirthDate = maxAge.HasValue ? DateTime.UtcNow.AddYears(-maxAge.Value) : DateTime.Parse("1800-01-01");

                AddCriteria(x => x.Winner.BirthDate >= minBirthDate && x.Winner.BirthDate <= maxBirthDate);
            }

            return this;
        }

        private void ApplyOrderBy(Pagination pagination)
        {
            if (string.IsNullOrEmpty(pagination.OrderBy))
            {
                return;
            }

            switch (pagination.OrderBy)
            {
                case "created":
                    ApplyOrderBy(p => p.Created, pagination.Descending);
                    break;
            }
        }

        #endregion
    }
}

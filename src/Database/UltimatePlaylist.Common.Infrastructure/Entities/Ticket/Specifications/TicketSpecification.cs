﻿#region Usings

using System;
using System.Linq;
using UltimatePlaylist.Common.Config;
using UltimatePlaylist.Common.Enums;
using UltimatePlaylist.Database.Infrastructure.Specifications;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Ticket.Specifications
{
    public class TicketSpecification : BaseSpecification<TicketEntity>
    {
        #region Constructor(s)

        public TicketSpecification(bool includeDeleted = false)
        {
            if (!includeDeleted)
            {
                AddCriteria(s => !s.IsDeleted);
            }
        }

        #endregion

        #region Filters

        public TicketSpecification ByExternalId(Guid externalId)
        {
            AddCriteria(s => s.ExternalId == externalId);

            return this;
        }

        public TicketSpecification ByExternalIds(Guid[] externalIds)
        {
            AddCriteria(s => externalIds.Contains(s.ExternalId));

            return this;
        }

        public TicketSpecification BySongHistoryUserExternalIdUsingSongRelation(Guid userExternalId)
        {
            AddCriteria(s => s.UserSongHistory.User.ExternalId == userExternalId);

            return this;
        }

        public TicketSpecification ByUserExternalIdUsingPlaylistRelation(Guid userExternalId)
        {
            AddCriteria(s => s.UserPlaylistSong.UserPlaylist.User.ExternalId == userExternalId);

            return this;
        }

        public TicketSpecification OnlyUsed()
        {
            AddCriteria(s => s.IsUsed);

            return this;
        }

        public TicketSpecification OnlyNotUsed()
        {
            AddCriteria(s => !s.IsUsed);

            return this;
        }

        public TicketSpecification ByType(TicketType ticketType)
        {
            AddCriteria(s => s.Type == ticketType);

            return this;
        }

        public TicketSpecification ByEarnedType(TicketEarnedType ticketEarnedType)
        {
            AddCriteria(s => s.EarnedType == ticketEarnedType);

            return this;
        }

        public TicketSpecification ByEarnedForPlaylistType()
        {
            AddCriteria(s => s.EarnedType != TicketEarnedType.AddedToSpotify && s.EarnedType != TicketEarnedType.AddedToAppleMusic);

            return this;
        }

        public TicketSpecification ByEarnedForSongType()
        {
            AddCriteria(s => s.EarnedType == TicketEarnedType.AddedToSpotify || s.EarnedType == TicketEarnedType.AddedToAppleMusic);

            return this;
        }

        public TicketSpecification ByUserSongHistoryExternalId(Guid userSongHistoryExternalId)
        {
            AddCriteria(s => s.UserSongHistory.ExternalId == userSongHistoryExternalId);

            return this;
        }

        public TicketSpecification ByPlaylistSongExternalId(Guid playlistSongExternalId)
        {
            AddCriteria(s => s.UserPlaylistSong.ExternalId == playlistSongExternalId);

            return this;
        }

        public TicketSpecification ByUserPlaylistSongId(long userPlaylistSongId)
        {
            AddCriteria(s => s.UserPlaylistSongId == userPlaylistSongId);
            return this;
        }

        public TicketSpecification ByTodaysTickets()
        {
            // TODO Temporal hack to test correct filtering
            string timezoneId = "US Eastern Standard Time";
            var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timezoneId); // converted utc timezone to EST timezone
            TimeZoneInfo targetTimezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            double offsetHours = targetTimezone.GetUtcOffset(DateTime.UtcNow).TotalHours;
            AddCriteria(s => s.Created.AddHours(offsetHours).Date.Equals(now.Date));

            return this;
        }
        
        #endregion

        #region Include

        public TicketSpecification WithUser()
        {
            AddInclude(s => s.UserSongHistory);
            AddInclude(s => s.UserSongHistory.User);

            return this;
        }

        public TicketSpecification WithUserByPlaylist()
        {
            AddInclude(s => s.UserPlaylistSong);
            AddInclude(s => s.UserPlaylistSong.UserPlaylist);
            AddInclude(s => s.UserPlaylistSong.UserPlaylist.User);

            return this;
        }

        #endregion
    }
}

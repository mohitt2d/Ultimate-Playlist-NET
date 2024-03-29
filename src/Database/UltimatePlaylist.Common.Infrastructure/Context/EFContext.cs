﻿#region Usings

using System.Data.Entity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UltimatePlaylist.Database.Infrastructure.Entities.Dsp;
using UltimatePlaylist.Database.Infrastructure.Entities.Identity;
using UltimatePlaylist.Database.Infrastructure.Entities.Playlist;
using UltimatePlaylist.Database.Infrastructure.Entities.Song;
using UltimatePlaylist.Database.Infrastructure.Entities.Ticket;
using UltimatePlaylist.Database.Infrastructure.Entities.UserSongHistory;
using UltimatePlaylist.Database.Infrastructure.Views;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Context
{
    public class EFContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        #region Constructor(s)

        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
            this.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
        }

        public EFContext()
        {
            this.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
        }

        #endregion

        #region DbSets

        public DbSet<GenderEntity> Genders { get; set; }

        public DbSet<GenreEntity> Genres { get; set; }

        public DbSet<UserDspEntity> UserDsps { get; set; }

        public DbSet<UserSongHistoryEntity> UserSongsHistory { get; set; }

        public DbSet<SongEntity> Songs { get; set; }

        public DbSet<PlaylistEntity> Playlists { get; set; }

        public DbSet<UserPlaylistEntity> UserPlaylists { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }

        public DbSet<UserPlaylistSongEntity> UserPlaylistSongs { get; set; }

        public DbSet<GeneralSongDataProcedureView> GeneralSongDataProcedureViews { get; set; }

        public DbSet<GeneralSongsCountProcedureView> GeneralSongsCountProcedureViews { get; set; }

        public DbSet<GeneralSongsAnalyticsFileInformationView> GeneralSongsAnalyticsFileInformationViews { get; set; }

        public DbSet<UserManagementProcedureView> UserManagementProcedureViews { get; set; }

        public DbSet<UserManagementProcedureViewCount> UserManagementProcedureCountViews { get; set; }

        public DbSet<LeaderboardRankingByTicketCountView> LeaderboardRankingByTicketCountViews { get; set; }

        public DbSet<LeaderboardRankingBySongCountView> LeaderboardRankingBySongCountViews { get; set; }

        public DbSet<ListenersStatisticsProcedureView> ListenersStatisticsProcedureViews { get; set; }

        #endregion

        #region Builder
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(InfrastructureAssembly).Assembly);
            builder.Entity<UserManagementProcedureViewCount>().HasNoKey();
            builder.Entity<ListenersStatisticsProcedureView>().HasNoKey();
            builder.Entity<LeaderboardRankingByTicketCountView>(eb => eb.ToView("LeaderboardTicketCountRanking"));
            builder.Entity<LeaderboardRankingBySongCountView>(eb => eb.ToView("LeaderboardSongCountRanking"));
            builder.Entity<SongPopularityView>(eb => eb.ToView("SongPopularity"));
        }

        #endregion
    }
}
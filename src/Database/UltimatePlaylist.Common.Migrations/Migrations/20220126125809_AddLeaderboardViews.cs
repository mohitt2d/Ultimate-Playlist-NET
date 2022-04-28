using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddLeaderboardViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
					Create Or ALTER VIEW [dbo].[LeaderboardSongCountRanking] AS
						Select 
							ROW_NUMBER() OVER (ORDER BY (Select Count(DISTINCT [UPS].[SongId]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Where [UP].UserId = [U].[Id] and [UPS].[IsFinished] = 1) desc) AS RankingPosition,
							[U].[Id],
							[U].[ExternalId],
							[U].[Created],
							[U].[Updated],
							[U].[IsDeleted],
							UPPER([U].[Name] + ' ' + [U].[LastName]) as FullName,
							UserName,
							(Select [Url] from [dbo].[File] as Avatar Where [Avatar].[Id] = [U].[AvatarFileId]) as AvatarUrl,
							(Select Count(DISTINCT [UPS].[SongId]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Where [UP].UserId = [U].[Id] and [UPS].[IsFinished] = 1 and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0) as SongCount
						from [dbo].[AspNetUsers] as U
						Inner Join [dbo].[AspNetUserRoles] as [USR] on [USR].[UserId] = [U].[Id]
						Inner Join [dbo].[AspNetRoles] as [R] on [USR].[RoleId] = [R].[Id]
						Where [U].[IsDeleted] = 0 and [R].[NormalizedName] = 'USER'");

			migrationBuilder.Sql(@"
					Create Or ALTER VIEW [dbo].[LeaderboardTicketCountRanking] AS
						Select
							ROW_NUMBER() OVER (ORDER BY (Select Count(DISTINCT [T].[Id]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Inner Join [dbo].[Ticket] as [T] on [T].[UserPlaylistSongId] = [UPS].Id
														Where [UP].UserId = [U].[Id] and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0) desc) AS RankingPosition,
							[U].[Id],
							[U].[ExternalId],
							[U].[Created],
							[U].[Updated],
							[U].[IsDeleted],
							UPPER([U].[Name] + ' ' + [U].[LastName]) as FullName,
							UserName,
							(Select [Url] from [dbo].[File] as Avatar Where [Avatar].[Id] = [U].[AvatarFileId]) as AvatarUrl,
							(Select Count(DISTINCT [T].[Id]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Inner Join [dbo].[Ticket] as [T] on [T].[UserPlaylistSongId] = [UPS].Id
														Where [UP].UserId = [U].[Id] and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0) as TicketCount
						from [dbo].[AspNetUsers] as U 
						Where [U].[IsDeleted] = 0 and Exists (SELECT 1 FROM [AspNetUserRoles] AS [UR] INNER JOIN [AspNetRoles] AS [R] ON [UR].[RoleId] = [R].[Id] WHERE ([U].[Id] = [UR].[UserId]) AND ([R].[Name] = N'User'))");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"Drop VIEW If EXISTS [dbo].[LeaderboardSongCountRanking]");
			migrationBuilder.Sql(@"Drop VIEW If EXISTS [dbo].[LeaderboardTicketCountRanking]");
        }
    }
}

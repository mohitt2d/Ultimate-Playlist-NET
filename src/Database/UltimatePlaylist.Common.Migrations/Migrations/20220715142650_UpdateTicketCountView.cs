using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class UpdateTicketCountView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
					Create Or ALTER VIEW [dbo].[LeaderboardTicketCountRanking] AS
						Select
							ROW_NUMBER() OVER (ORDER BY (Select Count(DISTINCT [T].[Id]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Inner Join [dbo].[Ticket] as [T] on ([T].[UserPlaylistSongId] = [UPS].Id)
														Where [UP].UserId = [U].[Id] and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0  AND [T].Type = 'Daily') desc) AS RankingPosition,
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
														Where [UP].UserId = [U].[Id] and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0 AND [T].Type = 'Daily') as TicketCount
						from [dbo].[AspNetUsers] as U 
						Where [U].[IsDeleted] = 0 and Exists (SELECT 1 FROM [AspNetUserRoles] AS [UR] INNER JOIN [AspNetRoles] AS [R] ON [UR].[RoleId] = [R].[Id] WHERE ([U].[Id] = [UR].[UserId]) AND ([R].[Name] = N'User'))");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"Drop VIEW If EXISTS [dbo].[LeaderboardTicketCountRanking]");
		}
    }
}

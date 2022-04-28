using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class SongCountViewUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
					Create Or ALTER VIEW [dbo].[LeaderboardSongCountRanking] AS
						Select 
							ROW_NUMBER() OVER (ORDER BY (Select Count([UPS].[SongId]) 
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
							(Select Count([UPS].[SongId]) 
														From [dbo].[UserPlaylistSong] as [UPS] 
														Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
														Where [UP].UserId = [U].[Id] and [UPS].[IsFinished] = 1 and [UPS].[IsDeleted] = 0 and [UP].[IsDeleted] = 0) as SongCount
						from [dbo].[AspNetUsers] as U
						Inner Join [dbo].[AspNetUserRoles] as [USR] on [USR].[UserId] = [U].[Id]
						Inner Join [dbo].[AspNetRoles] as [R] on [USR].[RoleId] = [R].[Id]
						Where [U].[IsDeleted] = 0 and [R].[NormalizedName] = 'USER'");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
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
		}
    }
}

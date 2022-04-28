using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class UserManagementAsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create Or Alter Procedure [dbo].[UserManagementView]
		@TimeRange dateTime2,
		@SortType nvarchar(50), 
		@Skip int = 0,
		@Take int = 20,
		@SearchValue nvarchar(100),
		@IsActive nvarchar(10)
        AS
		BEGIN
				SET NOCOUNT ON;

Declare @IsActiveBit as bit = CASE When @IsActive = '' Then Null Else CAST(@IsActive as bit) END;
Select
	[Id],
	[LastName],
	[Name],
	[ExternalId],
	[UserName],
	[LastActive],
	[IsActive],
	[BirthDate],
	ImageUrl,
	[TotalMinutesListened],
	[AvarageDailyPlays],
	[AvarageTimeListened] 
	From 
(Select 
	[U].[Id],
	[U].[LastName],
	[U].[Name],
	[U].[ExternalId],
	[U].[UserName],
	[U].[LastActive],
	[U].[IsActive],
	[U].[BirthDate],
	[F].[Url] as ImageUrl,
	IsNull((Select Cast((DatePart(mi, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))
			+ (DatePart(hh, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))*60)) as float)
			From UserPlaylistSong as UPS
			Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId] and  [UP].[UserId] = [U].[Id]
			Inner Join [dbo].[Song] as [SO] on [SO].[ID] = [UPS].[SongId]
			And (@TimeRange is null or [UP].[Created] >= @TimeRange)
			Where 
			[UPS].[IsDeleted] = 0
			AND [UP].[IsDeleted] = 0
			And [UPS].[IsFinished] = 1
			AND [SO].[IsDeleted] = 0),0) as [TotalMinutesListened],
	IsNull((Select (Cast(Count([UPS].[Id]) as float)/Case When Count(DISTINCT [UP].[ID]) = 0 Then 1 Else Count(DISTINCT [UP].[ID]) End)
			From [dbo].[UserPlaylistSong] as [UPS] 
			Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
			Where 
			[UP].[UserId] = [U].[Id]
			And [UPS].[IsFinished] = 1
			And (@TimeRange is null or [UPS].[Created] >= @TimeRange)
			), 0) as [AvarageDailyPlays],
	IsNull((Select 
			Cast((DatePart(mi, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))
			+ (DatePart(hh, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))*60)) as float)/ Count(DISTINCT [UP].[ID])
			From UserPlaylistSong as UPS
			Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId] and  [UP].[UserId] = [U].[Id]
			Inner Join [dbo].[Song] as [SO] on [SO].[ID] = [UPS].[SongId]
			And (@TimeRange is null or [UP].[Created] >= @TimeRange)
			Where 
			[UPS].[IsDeleted] = 0
			And [UPS].[IsFinished] = 1
			AND [UP].[IsDeleted] = 0
			AND [SO].[IsDeleted] = 0), 0) as [AvarageTimeListened]
From [dbo].[AspNetUsers] as [U]
Left Join [dbo].[File] as [F] on [F].[Id] = [U].[AvatarFileId]
Where ((@SearchValue = '' Or @SearchValue is null) Or 
	  ([U].[LastName] like '%'+@SearchValue+'%' Or [U].[Name] like '%'+@SearchValue+'%' Or [U].[UserName] like '%'+@SearchValue+'%'))
And [U].[IsDeleted] = 0
And Exists (SELECT 1 FROM [AspNetUserRoles] AS [UR] INNER JOIN [AspNetRoles] AS [R] ON [UR].[RoleId] = [R].[Id] WHERE ([U].[Id] = [UR].[UserId]) AND ([R].[Name] = N'User'))
And (@IsActiveBit is null Or @IsActiveBit = [U].[IsActive])
) as UserManagement
				   Order By
						CASE WHEN (@SortType = '' or @SortType = null) THEN Id END DESC,
						CASE WHEN (@SortType = 'TotalMinutesListened') THEN [TotalMinutesListened] END DESC,
						CASE WHEN (@SortType = 'AvarageTimeListened') THEN [AvarageTimeListened] END DESC,
						CASE WHEN (@SortType = 'AvarageDailyPlays') THEN [AvarageDailyPlays] END DESC
						OFFSET     @Skip ROWS
						FETCH NEXT @Take ROWS ONLY
End");
            migrationBuilder.Sql(@"Create Or Alter Procedure [dbo].[UserManagementViewCount]
		@SearchValue nvarchar(100),
		@IsActive nvarchar(10)
        AS
		BEGIN
				SET NOCOUNT ON;

Declare @IsActiveBit as bit = CASE When @IsActive = '' Then Null Else CAST(@IsActive as bit) END;
Select Count([U].[Id]) as UserCount
From [dbo].[AspNetUsers] as [U]
Left Join [dbo].[File] as [F] on [F].[Id] = [U].[AvatarFileId]
Where ((@SearchValue = '' Or @SearchValue is null) Or 
	  ([U].[LastName] like '%'+@SearchValue+'%' Or [U].[Name] like '%'+@SearchValue+'%' Or [U].[UserName] like '%'+@SearchValue+'%'))
And [U].[IsDeleted] = 0
And (@IsActiveBit is null Or @IsActiveBit = [U].[IsActive])
And Exists (SELECT 1 FROM [AspNetUserRoles] AS [UR] INNER JOIN [AspNetRoles] AS [R] ON [UR].[RoleId] = [R].[Id] WHERE ([U].[Id] = [UR].[UserId]) AND ([R].[Name] = N'User'))

End");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[UserManagementViewCount]");
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[UserManagementView]");
		}
    }
}

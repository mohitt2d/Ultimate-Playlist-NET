using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class ListenersStatisticsProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"Create Or Alter Procedure [dbo].[ListenersStatistics]
		@From dateTime2,
		@To dateTime2

		As 
		Begin

Declare @TotalListeners int;
Declare @TotalDailyListeners int;
Declare @AverageDailyPlaysPerUser float;
Declare @AverageTimeListenedUser float;
Declare @TotalMaxListeners int;
Declare @TotalAverageListeners float;

Set @TotalListeners = (Select Count([U].[Id]) From [dbo].[AspNetUsers] as [U]
Where IsDeleted = 0 
And Exists (SELECT 1 FROM [AspNetUserRoles] AS [UR] INNER JOIN [AspNetRoles] AS [R] ON [UR].[RoleId] = [R].[Id] WHERE ([U].[Id] = [UR].[UserId]) AND ([R].[Name] = N'User')))

Set @TotalMaxListeners = IsNull((Select Max(Listeners) From (Select Count([UP].[Id]) as Listeners,
		dateadd(DAY,0, datediff(day,0, UP.Created)) as created
		From 
		[dbo].[UserPlaylist] as [UP]
		Group by dateadd(DAY,0, datediff(day,0, UP.Created))) as Tabel), 0)

Set @AverageDailyPlaysPerUser =	IsNull((Select (Cast(Count([UPS].[Id]) as float)/ @TotalListeners)
			From [dbo].[UserPlaylistSong] as [UPS] 
			Where [UPS].[IsFinished] = 1
				And [UPS].[Created] > @From
				And [UPS].[Created] < @To
			), 0)

Set @AverageTimeListenedUser = IsNull((Select 
			Cast((DatePart(mi, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))
			+ (DatePart(hh, Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7)))*60)) as float)/ @TotalListeners
			From UserPlaylistSong as UPS
			Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
			Inner Join [dbo].[Song] as [SO] on [SO].[ID] = [UPS].[SongId]
			Where 
				[UPS].[IsDeleted] = 0
				And [UPS].[IsFinished] = 1
				And [UP].[IsDeleted] = 0
				And [SO].[IsDeleted] = 0
				And [UPS].[Created] > @From
				And [UPS].[Created] < @To), 0)

Set @TotalDailyListeners = IsNull((Select Count([UP].[Id]) as Listeners
		From 
		[dbo].[UserPlaylist] as [UP]
		Where 
			dateadd(DAY,0, datediff(day,0, UP.Created)) = DATEADD(day, -1, CAST(GETDATE() AS date))
		Group by dateadd(DAY,0, datediff(day,0, UP.Created))), 0)

Set @TotalAverageListeners = IsNull((Select Cast(Sum(Listeners) as float)/ 5  From (Select Count([UP].[Id]) as Listeners
		From 
		[dbo].[UserPlaylist] as [UP]
		Where 
			dateadd(DAY,0, datediff(day,0, UP.Created)) <= DATEADD(day, 1, CAST(GETDATE() AS date))
			and dateadd(DAY,0, datediff(day,0, UP.Created)) >= DATEADD(day, -5, CAST(GETDATE() AS date))
		Group by dateadd(DAY,0, datediff(day,0, UP.Created))) as T), 0)

Select @TotalListeners as TotalListeners,
	   @TotalMaxListeners as TotalMaxListeners,
	   @AverageDailyPlaysPerUser as AverageDailyPlaysPerUser,
	   @AverageTimeListenedUser as AverageTimeListenedUser,
	   @TotalDailyListeners as TotalDailyListeners,
	   @TotalAverageListeners as TotalAverageListeners

	   End");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[ListenersStatistics]");
		}
    }
}

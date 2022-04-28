using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class SongsAnalytics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
Create Or ALTER  PROCEDURE [dbo].[GetUserIds]
                    @BirthDateMin dateTime,
					@BirthDateMax dateTime,
					@Gender varchar(50),
					@ZipCode varchar(50)
                AS
        BEGIN
				SET NOCOUNT ON;
			    Select [U].[Id] From [dbo].[AspNetUsers] as [U]
				    Left Join [dbo].[Gender] as [G] on [G].[Id] = [U].[GenderId]
 				    Where 
				    [U].[BirthDate] <= @BirthDateMin
				    And  [U].[BirthDate] >= @BirthDateMax
				    And  ((@ZipCode = '' Or @ZipCode is null) Or [U].[ZipCode] = @ZipCode)
				    And ((@Gender = '' Or @Gender is null) Or [G].[Name] = @Gender)
        End");

            migrationBuilder.Sql(@"
Create Or ALTER PROCEDURE [dbo].[SongsAnalyticsCount]
					@Genres varchar(max),
					@Licensor varchar(max)
                AS
        BEGIN
				SET NOCOUNT ON;

            Select 
	               Count([S].[Id]) as SongsCount
	               From [dbo].[Song] as S
	               LEFT Join [dbo].[SongGenre] as [SG] on [SG].[SongId] = [S].[Id]  
	               LEFT Join [dbo].[Genre] as [G] on [G].[Id] = [SG].[GenreId]
	               Where [S].[IsDeleted] = 0 
	               And ((@Genres is null Or @Genres = '') Or @Genres like ('%'+[G].[Name] +'%'))
				   And (((@Licensor is null Or @Licensor = '') Or [S].[Licensor] like ('%'+ @Licensor +'%')))

        End
");

            migrationBuilder.Sql(@"
Create Or ALTER PROCEDURE [dbo].[SongsAnalytics]
                    @BirthDateMin dateTime,
					@BirthDateMax dateTime,
					@Gender varchar(50),
					@ZipCode varchar(50),
					@TimeRange dateTime null,
					@Skip int,
					@Take int,
					@SortType varchar(50),
					@Genres varchar(max),
					@Licensor varchar(max)
                AS
    BEGIN
				SET NOCOUNT ON;

		DECLARE @userIds TABLE(Id bigint not null);
		INSERT INTO @userIds exec [dbo].[GetUserIds]
				@BirthDateMin = @BirthDateMin,
				@BirthDateMax = @BirthDateMax,
				@Gender = @Gender,
				@ZipCode = @ZipCode


			Select 
				   [Id],
				   [ExternalId],
				   [Artist],
				   [Title],
				   [Licensor],
				   [Album],
				   [Genre],
				   [NumberOfTimesAddedToDSP],
				   [NumbersOfRate],
				   [AverageRating],
				   [UniquePlays],
				   [TotalTimeListened],
				   [CoverUrl] 
				   From
			(Select 
				   [S].[Id],
				   [S].[ExternalId],
				   [S].[Artist],
				   [S].[Title],
				   [S].[Licensor],
				   [S].[Album],
				   STUFF((Select ', ' + Name From (Select [Name] From [dbo].[Genre] as [G] 
								Left Join [dbo].[SongGenre] as [SG] on [SG].[SongId] = [S].[Id]
								Where [G].[Id] = [SG].[GenreId]) x FOR XML PATH('')), 1, 2, '') as Genre,
				   IsNull((Select Count([USG].[SongId]) 
								From [dbo].[UserSongHistory] as [USG] 
								Where [USG].[SongId] = [S].[Id] and
								([USG].[IsAddedToAppleMusic] = 1 or [USG].[IsAddedToSpotify] = 1)
								And [USG].[UserId] in (Select * From @userIds)
								And (@TimeRange is null or [USG].[Created] >= @TimeRange)
								Group By [USG].[SongId]), 0) as [NumberOfTimesAddedToDSP],
				   IsNull((Select Count([UPS].[ID]) 
								From [dbo].[UserPlaylistSong] as [UPS] 
								Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
								Where [UPS].[SongId] = [S].[Id] 
								AND [UPS].[Rating] > 0
								And [UP].[UserId] in (Select * From @userIds)
								And (@TimeRange is null or [UP].[Created] >= @TimeRange)), 0) as [NumbersOfRate],
				   (Select (Cast(SUM([UPS].[Rating]) as float)/Case When Count([UPS].[ID]) = 0 Then 1 Else Count([UPS].[Rating]) End)
						From [dbo].[UserPlaylistSong] as [UPS]
						Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
						Where 
								[UPS].[SongId] = [S].[Id] 
								AND [UPS].[Rating] > 0
								And [UP].[UserId] in (Select * From @userIds)
								And (@TimeRange is null or [UP].[Created] >= @TimeRange)) as [AverageRating],
				  IsNull((Select Count([UPS].[SongId])
								From [dbo].[UserPlaylistSong] as [UPS] 
								Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
								Where [UPS].[SongId] = [S].[Id]
								And [UP].[UserId] in (Select * From @userIds)
								And (@TimeRange is null or [UPS].[Created] >= @TimeRange)
								Group by [UPS].[SongId]), 0) as [UniquePlays],
				  (Select Cast(DateAdd(ms, Sum(DateDiff(ms, 0, Cast((ISNULL([UPS].[SkipDate], [SO].[Duration])) as time(7)))), 0) as time(7))
								From UserPlaylistSong as UPS
								Inner Join [dbo].[UserPlaylist] as [UP] on [UP].[Id] = [UPS].[UserPlaylistId]
								Inner Join [dbo].[Song] as [SO] on [SO].[ID] = [UPS].[SongId]
								Where [SO].[Id] = [S].[Id]
								And [UP].[UserId] in (Select * From @userIds)
								And (@TimeRange is null or [UP].[Created] >= @TimeRange)) as [TotalTimeListened],
				   (Select [F].[Url] From [dbo].[File] as [F] Where [F].[Id] = [S].[CoverFileId]) as [CoverUrl]
				   From [dbo].[Song] as S
				   LEFT Join [dbo].[SongGenre] as [SG] on [SG].[SongId] = [S].[Id]  
				   LEFT Join [dbo].[Genre] as [G] on [G].[Id] = [SG].[GenreId]
				   Where [S].[IsDeleted] = 0 
				   And ((@Genres is null Or @Genres = '') Or @Genres like ('%'+[G].[Name] +'%'))
				   And (((@Licensor is null Or @Licensor = '') Or [S].[Licensor] like ('%'+ @Licensor +'%')))
				   Group by 
						[S].[Artist],
						[S].[Title],
						[S].[Licensor],
						[S].[Album],
						[S].[Id],
						[S].[ExternalId],
						[S].[CoverFileId]) as SongAnalytics
				   Order By
						CASE WHEN (@SortType = '' or @SortType = null) THEN Id END DESC,
						CASE WHEN (@SortType = 'UniquePlays') THEN [UniquePlays] END DESC,
						CASE WHEN @SortType = 'TotalTimeListened' THEN [TotalTimeListened] END ASC,
						CASE WHEN (@SortType = 'NumberOfTimesAddedToDSP') THEN [NumberOfTimesAddedToDSP] END DESC,
						CASE WHEN (@SortType = 'NumbersOfRate') THEN [NumbersOfRate] END DESC
						OFFSET     @Skip ROWS
						FETCH NEXT @Take ROWS ONLY

	End
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[GetUserIds]");
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[SongsAnalytics]");
			migrationBuilder.Sql("Drop Procedure If EXISTS [dbo].[SongsAnalyticsCount]");
        }
    }
}

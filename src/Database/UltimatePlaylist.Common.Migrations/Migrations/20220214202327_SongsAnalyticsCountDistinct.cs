using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class SongsAnalyticsCountDistinct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Create Or ALTER PROCEDURE [dbo].[SongsAnalyticsCount]
					                @Genres varchar(max),
					                @Licensor varchar(max)
                                AS
                        BEGIN
				                SET NOCOUNT ON;

                            Select 
	                               Count(DISTINCT [S].[Id]) as SongsCount
	                               From [dbo].[Song] as S
	                               LEFT Join [dbo].[SongGenre] as [SG] on [SG].[SongId] = [S].[Id]  
	                               LEFT Join [dbo].[Genre] as [G] on [G].[Id] = [SG].[GenreId]
	                               Where [S].[IsDeleted] = 0 
	                               And ((@Genres is null Or @Genres = '') Or @Genres like ('%'+[G].[Name] +'%'))
				                   And (((@Licensor is null Or @Licensor = '') Or [S].[Licensor] like ('%'+ @Licensor +'%')))

                        End
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddSongPopularityView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
					Create Or ALTER VIEW [dbo].[SongPopularity] AS
						SELECT ROW_NUMBER() OVER (ORDER BY Count(IsFinished) desc) AS Position
						      ,[S].[Id]
							  ,[S].[ExternalId]
							  ,Max(([UPS].[Created])) as Created
							  ,Max(([UPS].[Updated])) as Updated
							  ,Cast(0 as bit) as IsDeleted
                              ,Count(IsFinished) as Amount
                              FROM [dbo].[UserPlaylistSong] as [UPS]
                              Inner Join [dbo].Song as [S] on [S].[Id] = [UPS].[SongId]
                              where IsFinished = 1 and [S].[IsDeleted] = 0 and [UPS].[IsDeleted] = 0
                              group by [S].[Id], [S].[ExternalId]");
		    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"Drop VIEW If EXISTS [dbo].[SongPopularity]");
		}
    }
}

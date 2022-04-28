using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class RemoveUnecessaryRatingPropertyFromUserSongTicketHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSongTicketHistoryEntity_Rating",
                table: "UserSongHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSongTicketHistoryEntity_Rating",
                table: "UserSongHistory",
                type: "int",
                nullable: true);
        }
    }
}

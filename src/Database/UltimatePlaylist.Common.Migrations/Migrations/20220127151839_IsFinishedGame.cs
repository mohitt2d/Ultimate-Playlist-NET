using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class IsFinishedGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Game");
        }
    }
}

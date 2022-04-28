using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class UserIsAgeVerified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAgeVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgeVerified",
                table: "AspNetUsers");
        }
    }
}

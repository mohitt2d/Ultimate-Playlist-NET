using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddSongToSpotifyPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAddedToSpotify",
                table: "UserSongHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserSpotifyIdentity",
                table: "UserDsp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserSpotifyPlaylistId",
                table: "UserDsp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SongSpotifyId",
                table: "SongDSP",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAddedToSpotify",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "UserSpotifyIdentity",
                table: "UserDsp");

            migrationBuilder.DropColumn(
                name: "UserSpotifyPlaylistId",
                table: "UserDsp");

            migrationBuilder.DropColumn(
                name: "SongSpotifyId",
                table: "SongDSP");
        }
    }
}

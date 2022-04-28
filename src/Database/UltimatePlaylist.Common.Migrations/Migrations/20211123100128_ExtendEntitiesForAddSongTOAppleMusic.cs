using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class ExtendEntitiesForAddSongTOAppleMusic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserSpotifyPlaylistId",
                table: "UserDsp",
                newName: "UserPlaylistId");

            migrationBuilder.RenameColumn(
                name: "SongSpotifyId",
                table: "SongDSP",
                newName: "SongDspId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAddedToAppleMusic",
                table: "UserSongHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAddedToAppleMusic",
                table: "UserSongHistory");

            migrationBuilder.RenameColumn(
                name: "UserPlaylistId",
                table: "UserDsp",
                newName: "UserSpotifyPlaylistId");

            migrationBuilder.RenameColumn(
                name: "SongDspId",
                table: "SongDSP",
                newName: "SongSpotifyId");
        }
    }
}

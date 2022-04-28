using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class IsFallback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSong_Playlist_PlylistId",
                table: "PlaylistSong");

            migrationBuilder.RenameColumn(
                name: "PlylistId",
                table: "PlaylistSong",
                newName: "PlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistSong_PlylistId",
                table: "PlaylistSong",
                newName: "IX_PlaylistSong_PlaylistId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFallback",
                table: "Playlist",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSong_Playlist_PlaylistId",
                table: "PlaylistSong",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSong_Playlist_PlaylistId",
                table: "PlaylistSong");

            migrationBuilder.DropColumn(
                name: "IsFallback",
                table: "Playlist");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "PlaylistSong",
                newName: "PlylistId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistSong_PlaylistId",
                table: "PlaylistSong",
                newName: "IX_PlaylistSong_PlylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSong_Playlist_PlylistId",
                table: "PlaylistSong",
                column: "PlylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

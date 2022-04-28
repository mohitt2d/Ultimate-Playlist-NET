using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class RefactorStoredCurrentPlaylistSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSongExternalId",
                table: "UserPlaylist");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "UserPlaylistSong",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "UserPlaylistSong");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentSongExternalId",
                table: "UserPlaylist",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

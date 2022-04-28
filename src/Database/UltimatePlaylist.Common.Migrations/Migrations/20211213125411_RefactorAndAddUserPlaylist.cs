using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class RefactorAndAddUserPlaylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Playlist_PlaylistId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserSongHistory");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "Ticket",
                newName: "UserPlaylistSongId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_PlaylistId",
                table: "Ticket",
                newName: "IX_Ticket_UserPlaylistSongId");

            migrationBuilder.CreateTable(
                name: "UserPlaylist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentSongExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlaylist", x => x.Id);
                    table.UniqueConstraint("AK_UserPlaylist_ExternalId", x => x.ExternalId);
                    table.ForeignKey(
                        name: "FK_UserPlaylist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPlaylistSong",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<long>(type: "bigint", nullable: true),
                    UserPlaylistId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    IsSkipped = table.Column<bool>(type: "bit", nullable: false),
                    SkipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlaylistSong", x => x.Id);
                    table.UniqueConstraint("AK_UserPlaylistSong_ExternalId", x => x.ExternalId);
                    table.ForeignKey(
                        name: "FK_UserPlaylistSong_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlaylistSong_UserPlaylist_UserPlaylistId",
                        column: x => x.UserPlaylistId,
                        principalTable: "UserPlaylist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylist_UserId",
                table: "UserPlaylist",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylistSong_SongId",
                table: "UserPlaylistSong",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylistSong_UserPlaylistId",
                table: "UserPlaylistSong",
                column: "UserPlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_UserPlaylistSong_UserPlaylistSongId",
                table: "Ticket",
                column: "UserPlaylistSongId",
                principalTable: "UserPlaylistSong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_UserPlaylistSong_UserPlaylistSongId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "UserPlaylistSong");

            migrationBuilder.DropTable(
                name: "UserPlaylist");

            migrationBuilder.RenameColumn(
                name: "UserPlaylistSongId",
                table: "Ticket",
                newName: "PlaylistId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_UserPlaylistSongId",
                table: "Ticket",
                newName: "IX_Ticket_PlaylistId");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "UserSongHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Playlist_PlaylistId",
                table: "Ticket",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

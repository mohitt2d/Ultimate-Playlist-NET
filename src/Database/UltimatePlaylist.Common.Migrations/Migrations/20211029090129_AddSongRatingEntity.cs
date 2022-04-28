using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddSongRatingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistInstagram",
                table: "Song");

            migrationBuilder.CreateTable(
                name: "SongRatingEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongRatingEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongRatingEntity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SongRatingEntity_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongRatingEntity_SongId",
                table: "SongRatingEntity",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_SongRatingEntity_UserId",
                table: "SongRatingEntity",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongRatingEntity");

            migrationBuilder.AddColumn<string>(
                name: "ArtistInstagram",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

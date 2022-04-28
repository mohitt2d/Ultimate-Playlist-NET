using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddSongRatingConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongRatingEntity_AspNetUsers_UserId",
                table: "SongRatingEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SongRatingEntity_Song_SongId",
                table: "SongRatingEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongRatingEntity",
                table: "SongRatingEntity");

            migrationBuilder.RenameTable(
                name: "SongRatingEntity",
                newName: "SongRating");

            migrationBuilder.RenameIndex(
                name: "IX_SongRatingEntity_UserId",
                table: "SongRating",
                newName: "IX_SongRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SongRatingEntity_SongId",
                table: "SongRating",
                newName: "IX_SongRating_SongId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "SongRating",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newsequentialid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "SongRating",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SongRating_ExternalId",
                table: "SongRating",
                column: "ExternalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongRating",
                table: "SongRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongRating_AspNetUsers_UserId",
                table: "SongRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SongRating_Song_SongId",
                table: "SongRating",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongRating_AspNetUsers_UserId",
                table: "SongRating");

            migrationBuilder.DropForeignKey(
                name: "FK_SongRating_Song_SongId",
                table: "SongRating");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SongRating_ExternalId",
                table: "SongRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongRating",
                table: "SongRating");

            migrationBuilder.RenameTable(
                name: "SongRating",
                newName: "SongRatingEntity");

            migrationBuilder.RenameIndex(
                name: "IX_SongRating_UserId",
                table: "SongRatingEntity",
                newName: "IX_SongRatingEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SongRating_SongId",
                table: "SongRatingEntity",
                newName: "IX_SongRatingEntity_SongId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "SongRatingEntity",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newsequentialid()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "SongRatingEntity",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongRatingEntity",
                table: "SongRatingEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongRatingEntity_AspNetUsers_UserId",
                table: "SongRatingEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SongRatingEntity_Song_SongId",
                table: "SongRatingEntity",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

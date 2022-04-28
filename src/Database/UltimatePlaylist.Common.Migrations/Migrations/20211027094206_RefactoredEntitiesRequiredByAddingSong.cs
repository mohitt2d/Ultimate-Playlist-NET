using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class RefactoredEntitiesRequiredByAddingSong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSong_Song_SongId",
                table: "PlaylistSong");

            migrationBuilder.DropForeignKey(
                name: "FK_SongDSP_Song_SongId",
                table: "SongDSP");

            migrationBuilder.DropForeignKey(
                name: "FK_SongGenre_Song_SongId",
                table: "SongGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SongSocialMedia_SocialMedia_SocialMediaId",
                table: "SongSocialMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_SongSocialMedia_Song_SongId",
                table: "SongSocialMedia");

            migrationBuilder.DropTable(
                name: "SocialMedia");

            migrationBuilder.DropIndex(
                name: "IX_SongSocialMedia_SocialMediaId",
                table: "SongSocialMedia");

            migrationBuilder.DropColumn(
                name: "SocialMediaId",
                table: "SongSocialMedia");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "StreamUrl",
                table: "Song");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "UserDsp",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongSocialMedia",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SongSocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "SongSocialMedia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongGenre",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SongGenre",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongDSP",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "DspType",
                table: "SongDSP",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "AudioFileId",
                table: "Song",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CoverFileId",
                table: "Song",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "PlaylistSong",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "JobState",
                table: "File",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobErrorCode",
                table: "File",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SongId",
                table: "File",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Song_AudioFileId",
                table: "Song",
                column: "AudioFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Song_CoverFileId",
                table: "Song",
                column: "CoverFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSong_Song_SongId",
                table: "PlaylistSong",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Song_File_AudioFileId",
                table: "Song",
                column: "AudioFileId",
                principalTable: "File",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_File_CoverFileId",
                table: "Song",
                column: "CoverFileId",
                principalTable: "File",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongDSP_Song_SongId",
                table: "SongDSP",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenre_Song_SongId",
                table: "SongGenre",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SongSocialMedia_Song_SongId",
                table: "SongSocialMedia",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSong_Song_SongId",
                table: "PlaylistSong");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_File_AudioFileId",
                table: "Song");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_File_CoverFileId",
                table: "Song");

            migrationBuilder.DropForeignKey(
                name: "FK_SongDSP_Song_SongId",
                table: "SongDSP");

            migrationBuilder.DropForeignKey(
                name: "FK_SongGenre_Song_SongId",
                table: "SongGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SongSocialMedia_Song_SongId",
                table: "SongSocialMedia");

            migrationBuilder.DropIndex(
                name: "IX_Song_AudioFileId",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Song_CoverFileId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SongSocialMedia");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "SongSocialMedia");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SongGenre");

            migrationBuilder.DropColumn(
                name: "AudioFileId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "CoverFileId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "File");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "UserDsp",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongSocialMedia",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SocialMediaId",
                table: "SongSocialMedia",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongGenre",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "SongDSP",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DspType",
                table: "SongDSP",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreamUrl",
                table: "Song",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SongId",
                table: "PlaylistSong",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobState",
                table: "File",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobErrorCode",
                table: "File",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SocialMedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedia", x => x.Id);
                    table.UniqueConstraint("AK_SocialMedia_ExternalId", x => x.ExternalId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongSocialMedia_SocialMediaId",
                table: "SongSocialMedia",
                column: "SocialMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSong_Song_SongId",
                table: "PlaylistSong",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongDSP_Song_SongId",
                table: "SongDSP",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenre_Song_SongId",
                table: "SongGenre",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongSocialMedia_SocialMedia_SocialMediaId",
                table: "SongSocialMedia",
                column: "SocialMediaId",
                principalTable: "SocialMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongSocialMedia_Song_SongId",
                table: "SongSocialMedia",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

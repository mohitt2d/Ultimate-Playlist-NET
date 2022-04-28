using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class SaveNotificationsEnabledInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CoverFileEntity_SongId",
                table: "File",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "File",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AvatarFileId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSmsPromotionalNotificationEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarFileId",
                table: "AspNetUsers",
                column: "AvatarFileId",
                unique: true,
                filter: "[AvatarFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_File_AvatarFileId",
                table: "AspNetUsers",
                column: "AvatarFileId",
                principalTable: "File",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_File_AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoverFileEntity_SongId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "AvatarFileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsNotificationEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsSmsPromotionalNotificationEnabled",
                table: "AspNetUsers");
        }
    }
}

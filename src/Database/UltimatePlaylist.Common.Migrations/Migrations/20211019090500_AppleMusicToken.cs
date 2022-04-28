using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AppleMusicToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDsps_AspNetUsers_UserId",
                table: "UserDsps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDsps",
                table: "UserDsps");

            migrationBuilder.RenameTable(
                name: "UserDsps",
                newName: "UserDsp");

            migrationBuilder.RenameIndex(
                name: "IX_UserDsps_UserId",
                table: "UserDsp",
                newName: "IX_UserDsp_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "UserDsp",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newsequentialid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "UserDsp",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccessTokenExpirationDate",
                table: "UserDsp",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserDsp_ExternalId",
                table: "UserDsp",
                column: "ExternalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDsp",
                table: "UserDsp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDsp_AspNetUsers_UserId",
                table: "UserDsp",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDsp_AspNetUsers_UserId",
                table: "UserDsp");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserDsp_ExternalId",
                table: "UserDsp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDsp",
                table: "UserDsp");

            migrationBuilder.RenameTable(
                name: "UserDsp",
                newName: "UserDsps");

            migrationBuilder.RenameIndex(
                name: "IX_UserDsp_UserId",
                table: "UserDsps",
                newName: "IX_UserDsps_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                table: "UserDsps",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newsequentialid()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "UserDsps",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccessTokenExpirationDate",
                table: "UserDsps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDsps",
                table: "UserDsps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDsps_AspNetUsers_UserId",
                table: "UserDsps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

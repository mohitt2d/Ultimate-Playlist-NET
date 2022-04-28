using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddTicketEntityAndRefactorUserSongHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "UserSongHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "TicketId",
                table: "UserSongHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UserSongHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserSongHistoryType",
                table: "UserSongHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserSongTicketHistoryEntity_Rating",
                table: "UserSongHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UserSongTicketHistoryId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.UniqueConstraint("AK_Ticket_ExternalId", x => x.ExternalId);
                    table.ForeignKey(
                        name: "FK_Ticket_UserSongHistory_UserSongTicketHistoryId",
                        column: x => x.UserSongTicketHistoryId,
                        principalTable: "UserSongHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserSongTicketHistoryId",
                table: "Ticket",
                column: "UserSongTicketHistoryId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "UserSongHistoryType",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "UserSongTicketHistoryEntity_Rating",
                table: "UserSongHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "UserSongHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

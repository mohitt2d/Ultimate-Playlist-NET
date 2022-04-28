using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class ChangeRelationBetweenTicketAndUserSongTicketHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_UserSongHistory_UserSongTicketHistoryId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_UserSongTicketHistoryId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "UserSongTicketHistoryId",
                table: "Ticket");

            migrationBuilder.CreateIndex(
                name: "IX_UserSongHistory_TicketId",
                table: "UserSongHistory",
                column: "TicketId",
                unique: true,
                filter: "[TicketId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSongHistory_Ticket_TicketId",
                table: "UserSongHistory",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSongHistory_Ticket_TicketId",
                table: "UserSongHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserSongHistory_TicketId",
                table: "UserSongHistory");

            migrationBuilder.AddColumn<long>(
                name: "UserSongTicketHistoryId",
                table: "Ticket",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserSongTicketHistoryId",
                table: "Ticket",
                column: "UserSongTicketHistoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_UserSongHistory_UserSongTicketHistoryId",
                table: "Ticket",
                column: "UserSongTicketHistoryId",
                principalTable: "UserSongHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class RefactoredUserSongHistoryAndTicketEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSongHistory_Ticket_TicketId",
                table: "UserSongHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserSongHistory_TicketId",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserSongHistory");

            migrationBuilder.DropColumn(
                name: "UserSongHistoryType",
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

            migrationBuilder.AddColumn<string>(
                name: "EarnedType",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UserSongHistoryId",
                table: "Ticket",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserSongHistoryId",
                table: "Ticket",
                column: "UserSongHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_UserSongHistory_UserSongHistoryId",
                table: "Ticket",
                column: "UserSongHistoryId",
                principalTable: "UserSongHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_UserSongHistory_UserSongHistoryId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_UserSongHistoryId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "EarnedType",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "UserSongHistoryId",
                table: "Ticket");

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
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserSongHistoryType",
                table: "UserSongHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}

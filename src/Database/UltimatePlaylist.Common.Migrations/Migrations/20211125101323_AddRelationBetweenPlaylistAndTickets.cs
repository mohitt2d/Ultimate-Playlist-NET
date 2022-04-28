using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimatePlaylist.Database.Migrations.Migrations
{
    public partial class AddRelationBetweenPlaylistAndTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlaylistId",
                table: "Ticket",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_PlaylistId",
                table: "Ticket",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Playlist_PlaylistId",
                table: "Ticket",
                column: "PlaylistId",
                principalTable: "Playlist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Playlist_PlaylistId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_PlaylistId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Ticket");
        }
    }
}

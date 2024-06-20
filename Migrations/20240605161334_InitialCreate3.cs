using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euro2024REST.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team1Name",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team2Name",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Team_TeamName",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_TeamName",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Match_Team1Name",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_Team2Name",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "TeamName",
                table: "Player",
                newName: "Team");

            migrationBuilder.RenameColumn(
                name: "Team2Name",
                table: "Match",
                newName: "Team2");

            migrationBuilder.RenameColumn(
                name: "Team1Name",
                table: "Match",
                newName: "Team1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team",
                table: "Player",
                newName: "TeamName");

            migrationBuilder.RenameColumn(
                name: "Team2",
                table: "Match",
                newName: "Team2Name");

            migrationBuilder.RenameColumn(
                name: "Team1",
                table: "Match",
                newName: "Team1Name");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamName",
                table: "Player",
                column: "TeamName");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Team1Name",
                table: "Match",
                column: "Team1Name");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Team2Name",
                table: "Match",
                column: "Team2Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team1Name",
                table: "Match",
                column: "Team1Name",
                principalTable: "Team",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team2Name",
                table: "Match",
                column: "Team2Name",
                principalTable: "Team",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamName",
                table: "Player",
                column: "TeamName",
                principalTable: "Team",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Euro2024REST.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "Team2Name",
                table: "Match",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Team1Name",
                table: "Match",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team1Name",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Team_Team2Name",
                table: "Match");

            migrationBuilder.AlterColumn<string>(
                name: "Team2Name",
                table: "Match",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Team1Name",
                table: "Match",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team1Name",
                table: "Match",
                column: "Team1Name",
                principalTable: "Team",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Team_Team2Name",
                table: "Match",
                column: "Team2Name",
                principalTable: "Team",
                principalColumn: "Name");
        }
    }
}

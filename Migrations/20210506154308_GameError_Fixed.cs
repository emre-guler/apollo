using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollo.Migrations
{
    public partial class GameError_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "OldTeams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "OldTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

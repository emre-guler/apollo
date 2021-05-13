using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollo.Migrations
{
    public partial class GameColumns_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LolContact",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamContact",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitchContact",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValorantContact",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LolContact",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SteamContact",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TwitchContact",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ValorantContact",
                table: "Players");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollo.Migrations
{
    public partial class OldTeamDateData_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "OldTeams");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedAt",
                table: "OldTeams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "OldTeams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "OldTeams");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "OldTeams");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "OldTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

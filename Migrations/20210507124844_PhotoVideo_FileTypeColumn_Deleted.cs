using Microsoft.EntityFrameworkCore.Migrations;

namespace Apollo.Migrations
{
    public partial class PhotoVideo_FileTypeColumn_Deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Photos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

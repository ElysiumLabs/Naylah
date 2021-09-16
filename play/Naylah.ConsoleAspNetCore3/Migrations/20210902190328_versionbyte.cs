using Microsoft.EntityFrameworkCore.Migrations;

namespace Naylah.ConsoleAspNetCore.Migrations
{
    public partial class versionbyte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Test",
                table: "Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                schema: "Test",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

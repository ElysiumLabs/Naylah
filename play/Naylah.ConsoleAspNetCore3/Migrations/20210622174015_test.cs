using Microsoft.EntityFrameworkCore.Migrations;

namespace Naylah.ConsoleAspNetCore.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name_Full",
                schema: "Test",
                table: "Person",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_Full",
                schema: "Test",
                table: "Person");
        }
    }
}

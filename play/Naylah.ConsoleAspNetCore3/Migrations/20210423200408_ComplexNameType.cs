using Microsoft.EntityFrameworkCore.Migrations;

namespace Naylah.ConsoleAspNetCore.Migrations
{
    public partial class ComplexNameType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Test",
                table: "Person",
                newName: "Name_LastName");

            migrationBuilder.AddColumn<string>(
                name: "Name_FirstName",
                schema: "Test",
                table: "Person",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_FirstName",
                schema: "Test",
                table: "Person");

            migrationBuilder.RenameColumn(
                name: "Name_LastName",
                schema: "Test",
                table: "Person",
                newName: "Name");
        }
    }
}

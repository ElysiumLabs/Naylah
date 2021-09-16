using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Naylah.ConsoleAspNetCore.Migrations
{
    public partial class versionbyte2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                schema: "Test",
                table: "Person",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Test",
                table: "Person");
        }
    }
}

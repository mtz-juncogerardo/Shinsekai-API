using Microsoft.EntityFrameworkCore.Migrations;

namespace Shinsekai_API.Migrations
{
    public partial class ShinsekaiMigration_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalSerial",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalSerial",
                table: "Articles");
        }
    }
}

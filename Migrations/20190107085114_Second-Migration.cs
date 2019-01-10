using Microsoft.EntityFrameworkCore.Migrations;

namespace Calendar.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "Calendars",
                newName: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Calendars",
                newName: "Owner");
        }
    }
}

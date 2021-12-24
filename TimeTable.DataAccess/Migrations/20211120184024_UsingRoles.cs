using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTable.DataAccess.Migrations
{
    public partial class UsingRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "People");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "People",
                type: "bit",
                nullable: true);
        }
    }
}

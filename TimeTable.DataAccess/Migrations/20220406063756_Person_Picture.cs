using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.DataAccess.Migrations
{
    public partial class Person_Picture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "People");
        }
    }
}

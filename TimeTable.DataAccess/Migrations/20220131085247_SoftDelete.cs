using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTable.DataAccess.Migrations
{
    public partial class SoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TimeRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecords_IsDeleted",
                table: "TimeRecords",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_People_IsDeleted",
                table: "People",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Company_IsDeleted",
                table: "Company",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeRecords_IsDeleted",
                table: "TimeRecords");

            migrationBuilder.DropIndex(
                name: "IX_People_IsDeleted",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Company_IsDeleted",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TimeRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Company");
        }
    }
}

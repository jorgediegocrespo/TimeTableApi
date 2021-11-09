using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTable.DataAccess.Migrations
{
    public partial class _20211109PersonWithoutCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Companies_CompanyId",
                table: "People");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Companies_CompanyId",
                table: "People",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Companies_CompanyId",
                table: "People");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "People",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Companies_CompanyId",
                table: "People",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

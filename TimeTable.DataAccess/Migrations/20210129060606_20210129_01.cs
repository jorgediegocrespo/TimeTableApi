using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTable.DataAccess.Migrations
{
    public partial class _20210129_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDays_Company_CompanyId",
                table: "BankDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_People_PersonRequestingId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Company_CompanyId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationDays_People_PersonId",
                table: "VacationDays");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "VacationDays",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "People",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonRequestingId",
                table: "Holidays",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Company",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BankDays",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VacationDays_Year",
                table: "VacationDays",
                column: "Year",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_Name",
                table: "People",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankDays_Company_CompanyId",
                table: "BankDays",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_People_PersonRequestingId",
                table: "Holidays",
                column: "PersonRequestingId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Company_CompanyId",
                table: "People",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationDays_People_PersonId",
                table: "VacationDays",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDays_Company_CompanyId",
                table: "BankDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_People_PersonRequestingId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Company_CompanyId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_VacationDays_People_PersonId",
                table: "VacationDays");

            migrationBuilder.DropIndex(
                name: "IX_VacationDays_Year",
                table: "VacationDays");

            migrationBuilder.DropIndex(
                name: "IX_People_Name",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Company_Name",
                table: "Company");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "VacationDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "People",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PersonRequestingId",
                table: "Holidays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BankDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BankDays_Company_CompanyId",
                table: "BankDays",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_People_PersonRequestingId",
                table: "Holidays",
                column: "PersonRequestingId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Company_CompanyId",
                table: "People",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacationDays_People_PersonId",
                table: "VacationDays",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

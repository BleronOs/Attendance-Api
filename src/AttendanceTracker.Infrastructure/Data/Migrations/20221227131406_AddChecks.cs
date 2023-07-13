using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_managment_employee_EmployeeId",
                table: "employee_managment");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_EmployeeId",
                table: "employee_managment");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "employee_managment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "birthdate",
                table: "employee",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "admin_id",
                table: "check",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "check",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "server_datetime",
                table: "check",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_managment_employee_employee_id",
                table: "employee_managment",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_managment_employee_employee_id",
                table: "employee_managment");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment");

            migrationBuilder.DropColumn(
                name: "admin_id",
                table: "check");

            migrationBuilder.DropColumn(
                name: "note",
                table: "check");

            migrationBuilder.DropColumn(
                name: "server_datetime",
                table: "check");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "employee_managment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "birthdate",
                table: "employee",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_employee_managment_EmployeeId",
                table: "employee_managment",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_managment_employee_EmployeeId",
                table: "employee_managment",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

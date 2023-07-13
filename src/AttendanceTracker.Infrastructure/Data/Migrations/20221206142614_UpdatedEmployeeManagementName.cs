using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEmployeeManagementName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employe_managment_employee_EmployeeId",
                table: "employe_managment");

            migrationBuilder.DropForeignKey(
                name: "FK_employe_managment_manager_manager_id",
                table: "employe_managment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employe_managment",
                table: "employe_managment");

            migrationBuilder.RenameTable(
                name: "employe_managment",
                newName: "employee_managment");

            migrationBuilder.RenameIndex(
                name: "IX_employe_managment_manager_id",
                table: "employee_managment",
                newName: "IX_employee_managment_manager_id");

            migrationBuilder.RenameIndex(
                name: "IX_employe_managment_EmployeeId",
                table: "employee_managment",
                newName: "IX_employee_managment_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employee_managment",
                table: "employee_managment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_managment_employee_EmployeeId",
                table: "employee_managment",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_managment_manager_manager_id",
                table: "employee_managment",
                column: "manager_id",
                principalTable: "manager",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_managment_employee_EmployeeId",
                table: "employee_managment");

            migrationBuilder.DropForeignKey(
                name: "FK_employee_managment_manager_manager_id",
                table: "employee_managment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employee_managment",
                table: "employee_managment");

            migrationBuilder.RenameTable(
                name: "employee_managment",
                newName: "employe_managment");

            migrationBuilder.RenameIndex(
                name: "IX_employee_managment_manager_id",
                table: "employe_managment",
                newName: "IX_employe_managment_manager_id");

            migrationBuilder.RenameIndex(
                name: "IX_employee_managment_EmployeeId",
                table: "employe_managment",
                newName: "IX_employe_managment_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employe_managment",
                table: "employe_managment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_employe_managment_employee_EmployeeId",
                table: "employe_managment",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employe_managment_manager_manager_id",
                table: "employe_managment",
                column: "manager_id",
                principalTable: "manager",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

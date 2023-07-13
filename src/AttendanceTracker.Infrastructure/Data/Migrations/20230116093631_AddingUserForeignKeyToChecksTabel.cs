using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserForeignKeyToChecksTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_manager_employee_id",
                table: "manager");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment");

            migrationBuilder.AlterColumn<string>(
                name: "admin_id",
                table: "check",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_manager_employee_id",
                table: "manager",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_check_admin_id",
                table: "check",
                column: "admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_check_user_admin_id",
                table: "check",
                column: "admin_id",
                principalTable: "user",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_check_user_admin_id",
                table: "check");

            migrationBuilder.DropIndex(
                name: "IX_manager_employee_id",
                table: "manager");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment");

            migrationBuilder.DropIndex(
                name: "IX_check_admin_id",
                table: "check");

            migrationBuilder.AlterColumn<int>(
                name: "admin_id",
                table: "check",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_manager_employee_id",
                table: "manager",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment",
                column: "employee_id");
        }
    }
}

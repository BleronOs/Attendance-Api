using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusColumnTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_job_position_position_id",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_manager_employee_id",
                table: "manager");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment");

            migrationBuilder.DropIndex(
                name: "IX_employee_position_id",
                table: "employee");

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "manager",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "JobPositionId",
                table: "employee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "employee",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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
                name: "IX_employee_JobPositionId",
                table: "employee",
                column: "JobPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_job_position_JobPositionId",
                table: "employee",
                column: "JobPositionId",
                principalTable: "job_position",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_job_position_JobPositionId",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_manager_employee_id",
                table: "manager");

            migrationBuilder.DropIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment");

            migrationBuilder.DropIndex(
                name: "IX_employee_JobPositionId",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "status",
                table: "manager");

            migrationBuilder.DropColumn(
                name: "JobPositionId",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "status",
                table: "employee");

            migrationBuilder.CreateIndex(
                name: "IX_manager_employee_id",
                table: "manager",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_managment_employee_id",
                table: "employee_managment",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_position_id",
                table: "employee",
                column: "position_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_job_position_position_id",
                table: "employee",
                column: "position_id",
                principalTable: "job_position",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

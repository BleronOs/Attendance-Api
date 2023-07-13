using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class creatingUserIdToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "employee",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_employee_userId",
                table: "employee",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_user_userId",
                table: "employee",
                column: "userId",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_user_userId",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_userId",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "employee");
        }
    }
}

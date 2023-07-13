using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCardIdToEmployeeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_card_employee_employee_id",
                table: "card");

            migrationBuilder.DropIndex(
                name: "IX_card_employee_id",
                table: "card");

            migrationBuilder.DropColumn(
                name: "employee_id",
                table: "card");

            migrationBuilder.AddColumn<int>(
                name: "card_id",
                table: "employee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "card_ref_id",
                table: "card",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_employee_card_id",
                table: "employee",
                column: "card_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_card_card_id",
                table: "employee",
                column: "card_id",
                principalTable: "card",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_card_card_id",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_card_id",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "card_id",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "card_ref_id",
                table: "card");

            migrationBuilder.AddColumn<int>(
                name: "employee_id",
                table: "card",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_card_employee_id",
                table: "card",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_card_employee_employee_id",
                table: "card",
                column: "employee_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

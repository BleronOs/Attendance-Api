using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrimaryKeyFromModuleIdToModulesAccessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_modules_access",
                table: "modules_access");

            migrationBuilder.CreateIndex(
                name: "IX_modules_access_module_id",
                table: "modules_access",
                column: "module_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_modules_access_module_id",
                table: "modules_access");

            migrationBuilder.AddPrimaryKey(
                name: "PK_modules_access",
                table: "modules_access",
                column: "module_id");
        }
    }
}

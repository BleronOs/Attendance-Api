using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyToModulesAccessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "role_id",
                table: "modules_access",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "module_id",
                table: "modules_access",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_modules_access_role_id",
                table: "modules_access",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_modules_access_modules_module_id",
                table: "modules_access",
                column: "module_id",
                principalTable: "modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_modules_access_role_role_id",
                table: "modules_access",
                column: "role_id",
                principalTable: "role",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_modules_access_modules_module_id",
                table: "modules_access");

            migrationBuilder.DropForeignKey(
                name: "FK_modules_access_role_role_id",
                table: "modules_access");

            migrationBuilder.DropIndex(
                name: "IX_modules_access_role_id",
                table: "modules_access");

            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "modules_access",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "module_id",
                table: "modules_access",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}

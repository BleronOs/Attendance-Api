using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AttendanceTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRemarksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "card",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            //migrationBuilder.AddColumn<string>(
            //    name: "reason_note",
            //    table: "card",
            //    type: "text",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "remarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeid = table.Column<int>(name: "employee_id", type: "integer", nullable: false),
                    shenime = table.Column<string>(type: "text", nullable: false),
                    inserteddatetime = table.Column<DateTime>(name: "inserted_datetime", type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_remarks", x => x.id);
                    table.ForeignKey(
                        name: "FK_remarks_employee_employee_id",
                        column: x => x.employeeid,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_remarks_employee_id",
                table: "remarks",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "remarks");

            //migrationBuilder.DropColumn(
            //    name: "reason_note",
            //    table: "card");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "card",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

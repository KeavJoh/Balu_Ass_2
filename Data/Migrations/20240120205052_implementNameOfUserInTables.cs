using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balu_Ass_2.Migrations
{
    /// <inheritdoc />
    public partial class implementNameOfUserInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeregistrationBy",
                table: "ChildWithdrawnDeregistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WithdrawnBy",
                table: "ChildWithdrawnDeregistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeregistrationBy",
                table: "ChildExpiredDeregistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeregistrationBy",
                table: "ChildDeregistrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeregistrationBy",
                table: "ChildWithdrawnDeregistrations");

            migrationBuilder.DropColumn(
                name: "WithdrawnBy",
                table: "ChildWithdrawnDeregistrations");

            migrationBuilder.DropColumn(
                name: "DeregistrationBy",
                table: "ChildExpiredDeregistrations");

            migrationBuilder.DropColumn(
                name: "DeregistrationBy",
                table: "ChildDeregistrations");
        }
    }
}

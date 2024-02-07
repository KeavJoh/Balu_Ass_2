using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balu_Ass_2.Migrations
{
    /// <inheritdoc />
    public partial class addChildrenGroupInChildDeregistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildrenGroup",
                table: "ChildDeregistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildrenGroup",
                table: "ChildDeregistrations");
        }
    }
}

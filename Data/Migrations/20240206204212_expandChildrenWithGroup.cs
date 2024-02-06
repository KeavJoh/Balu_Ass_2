using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balu_Ass_2.Migrations
{
    /// <inheritdoc />
    public partial class expandChildrenWithGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Childrens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Childrens");
        }
    }
}

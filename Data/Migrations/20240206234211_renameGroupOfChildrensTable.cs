using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balu_Ass_2.Migrations
{
    /// <inheritdoc />
    public partial class renameGroupOfChildrensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Group",
                table: "Childrens",
                newName: "ChildrenGroup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChildrenGroup",
                table: "Childrens",
                newName: "Group");
        }
    }
}

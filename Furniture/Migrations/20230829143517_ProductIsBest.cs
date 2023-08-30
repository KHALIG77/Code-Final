using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furniture.Migrations
{
    public partial class ProductIsBest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsNew",
                table: "Products",
                newName: "IsBest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBest",
                table: "Products",
                newName: "IsNew");
        }
    }
}

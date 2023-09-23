using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furniture.Migrations
{
    public partial class BrandAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "Features");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Brands");

            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

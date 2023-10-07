using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furniture.Migrations
{
    public partial class AlterSlider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForAbout",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForAbout",
                table: "Sliders");
        }
    }
}

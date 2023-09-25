using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Furniture.Migrations
{
    public partial class AlterInstagram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "InstagramPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "InstagramPhotos");
        }
    }
}

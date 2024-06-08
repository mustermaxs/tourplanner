using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Coordinate_Column_Mismatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Maps");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Tiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Tiles");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Maps",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

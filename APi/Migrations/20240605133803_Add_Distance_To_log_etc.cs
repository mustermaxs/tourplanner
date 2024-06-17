using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Distance_To_log_etc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Tours");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Lattitude",
                table: "Tours",
                newName: "Coordinates_Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coordinates_Latitude",
                table: "Tours",
                newName: "Coordinates_Lattitude");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Tours",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}

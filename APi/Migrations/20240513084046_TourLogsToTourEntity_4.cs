using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class TourLogsToTourEntity_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "TourLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourId",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "TourId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

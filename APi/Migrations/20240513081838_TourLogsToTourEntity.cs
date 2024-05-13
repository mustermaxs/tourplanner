using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class TourLogsToTourEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TourLogs",
                keyColumn: "TourLogId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tours",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "TourId1",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId1",
                table: "TourLogs",
                column: "TourId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId1",
                table: "TourLogs",
                column: "TourId1",
                principalTable: "Tours",
                principalColumn: "TourId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId1",
                table: "TourLogs");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId1",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "TourId1",
                table: "TourLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tours",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "TourId", "ChildFriendliness", "Description", "Distance", "EstimatedTime", "From", "ImagePath", "Name", "Popularity", "To", "TransportType" },
                values: new object[] { 1, 4f, "This is an example tour 1.", 100f, 2f, "Origin 1", "example1.jpg", "Example Tour 1", 4.5f, "Destination 1", 1 });

            migrationBuilder.InsertData(
                table: "TourLogs",
                columns: new[] { "TourLogId", "Comment", "Date", "Difficulty", "Duration", "Rating", "TourId" },
                values: new object[] { 1, "This was a great tour!", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3.5f, 2f, 4f, 1 });
        }
    }
}

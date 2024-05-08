using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Migration_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "TourId", "ChildFriendliness", "Description", "Distance", "EstimatedTime", "From", "ImagePath", "Name", "Popularity", "To" },
                values: new object[] { 1, 4f, "This is an example tour 1.", 100f, new TimeSpan(0, 2, 0, 0, 0), "Origin 1", "example1.jpg", "Example Tour 1", 4.5f, "Destination 1" });

            migrationBuilder.InsertData(
                table: "TourLogs",
                columns: new[] { "TourLogId", "Comment", "Difficulty", "Duration", "Rating", "TourId" },
                values: new object[] { 1, "This was a great tour!", 3.5f, new TimeSpan(0, 1, 0, 0, 0), 4f, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TourLogs",
                keyColumn: "TourLogId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1);
        }
    }
}

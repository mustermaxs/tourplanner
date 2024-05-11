using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Change_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "EstimatedTime",
                table: "Tours",
                type: "real",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<float>(
                name: "Duration",
                table: "TourLogs",
                type: "real",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TourLogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TourLogs",
                keyColumn: "TourLogId",
                keyValue: 1,
                columns: new[] { "Date", "Duration" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2f });

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                column: "EstimatedTime",
                value: 2f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "TourLogs");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EstimatedTime",
                table: "Tours",
                type: "interval",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "TourLogs",
                type: "interval",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.UpdateData(
                table: "TourLogs",
                keyColumn: "TourLogId",
                keyValue: 1,
                column: "Duration",
                value: new TimeSpan(0, 1, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Tours",
                keyColumn: "TourId",
                keyValue: 1,
                column: "EstimatedTime",
                value: new TimeSpan(0, 2, 0, 0, 0));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Map_Tile_Bbox_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tile_Maps_MapId",
                table: "Tile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tile",
                table: "Tile");

            migrationBuilder.RenameTable(
                name: "Tile",
                newName: "Tiles");

            migrationBuilder.RenameIndex(
                name: "IX_Tile_MapId",
                table: "Tiles",
                newName: "IX_Tiles_MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tiles",
                table: "Tiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiles_Maps_MapId",
                table: "Tiles",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiles_Maps_MapId",
                table: "Tiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tiles",
                table: "Tiles");

            migrationBuilder.RenameTable(
                name: "Tiles",
                newName: "Tile");

            migrationBuilder.RenameIndex(
                name: "IX_Tiles_MapId",
                table: "Tile",
                newName: "IX_Tile_MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tile",
                table: "Tile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tile_Maps_MapId",
                table: "Tile",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class FixReader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Reader_tb_Location_LocationNavigationId",
                table: "tb_Reader");

            migrationBuilder.DropIndex(
                name: "IX_tb_Reader_LocationNavigationId",
                table: "tb_Reader");

            migrationBuilder.DropColumn(
                name: "LocationNavigationId",
                table: "tb_Reader");

            migrationBuilder.AlterColumn<string>(
                name: "loc_id",
                table: "tb_Reader",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Reader_loc_id",
                table: "tb_Reader",
                column: "loc_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Reader_tb_Location_loc_id",
                table: "tb_Reader",
                column: "loc_id",
                principalTable: "tb_Location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Reader_tb_Location_loc_id",
                table: "tb_Reader");

            migrationBuilder.DropIndex(
                name: "IX_tb_Reader_loc_id",
                table: "tb_Reader");

            migrationBuilder.AlterColumn<string>(
                name: "loc_id",
                table: "tb_Reader",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "LocationNavigationId",
                table: "tb_Reader",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Reader_LocationNavigationId",
                table: "tb_Reader",
                column: "LocationNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Reader_tb_Location_LocationNavigationId",
                table: "tb_Reader",
                column: "LocationNavigationId",
                principalTable: "tb_Location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

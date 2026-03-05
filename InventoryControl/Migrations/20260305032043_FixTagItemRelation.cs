using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class FixTagItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Tag_tb_Item_ItemId",
                table: "tb_Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Tag_tb_Location_LocationId",
                table: "tb_Tag");

            migrationBuilder.DropIndex(
                name: "IX_tb_Tag_ItemId",
                table: "tb_Tag");

            migrationBuilder.DropIndex(
                name: "IX_tb_Tag_LocationId",
                table: "tb_Tag");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "tb_Tag");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "tb_Tag");

            migrationBuilder.AlterColumn<string>(
                name: "loc_id",
                table: "tb_Tag",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "itm_id",
                table: "tb_Tag",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Tag_itm_id",
                table: "tb_Tag",
                column: "itm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Tag_loc_id",
                table: "tb_Tag",
                column: "loc_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Tag_tb_Item_itm_id",
                table: "tb_Tag",
                column: "itm_id",
                principalTable: "tb_Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Tag_tb_Location_loc_id",
                table: "tb_Tag",
                column: "loc_id",
                principalTable: "tb_Location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Tag_tb_Item_itm_id",
                table: "tb_Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_Tag_tb_Location_loc_id",
                table: "tb_Tag");

            migrationBuilder.DropIndex(
                name: "IX_tb_Tag_itm_id",
                table: "tb_Tag");

            migrationBuilder.DropIndex(
                name: "IX_tb_Tag_loc_id",
                table: "tb_Tag");

            migrationBuilder.AlterColumn<string>(
                name: "loc_id",
                table: "tb_Tag",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "itm_id",
                table: "tb_Tag",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "tb_Tag",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "tb_Tag",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_Tag_ItemId",
                table: "tb_Tag",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Tag_LocationId",
                table: "tb_Tag",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Tag_tb_Item_ItemId",
                table: "tb_Tag",
                column: "ItemId",
                principalTable: "tb_Item",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Tag_tb_Location_LocationId",
                table: "tb_Tag",
                column: "LocationId",
                principalTable: "tb_Location",
                principalColumn: "id");
        }
    }
}

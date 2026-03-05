using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class FixHistoryPrintRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_History_Print_tb_Item_ItemId",
                table: "tb_History_Print");

            migrationBuilder.DropIndex(
                name: "IX_tb_History_Print_ItemId",
                table: "tb_History_Print");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "tb_History_Print");

            migrationBuilder.AlterColumn<string>(
                name: "itm_id",
                table: "tb_History_Print",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_History_Print_itm_id",
                table: "tb_History_Print",
                column: "itm_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_History_Print_tb_Item_itm_id",
                table: "tb_History_Print",
                column: "itm_id",
                principalTable: "tb_Item",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_History_Print_tb_Item_itm_id",
                table: "tb_History_Print");

            migrationBuilder.DropIndex(
                name: "IX_tb_History_Print_itm_id",
                table: "tb_History_Print");

            migrationBuilder.AlterColumn<string>(
                name: "itm_id",
                table: "tb_History_Print",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "tb_History_Print",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_History_Print_ItemId",
                table: "tb_History_Print",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_History_Print_tb_Item_ItemId",
                table: "tb_History_Print",
                column: "ItemId",
                principalTable: "tb_Item",
                principalColumn: "id");
        }
    }
}

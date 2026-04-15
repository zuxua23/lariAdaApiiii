using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryControl.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleToPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "module_id",
                table: "tb_Permission",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tb_Module",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    module_key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    module_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Module", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Permission_module_id",
                table: "tb_Permission",
                column: "module_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_Permission_tb_Module_module_id",
                table: "tb_Permission",
                column: "module_id",
                principalTable: "tb_Module",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_Permission_tb_Module_module_id",
                table: "tb_Permission");

            migrationBuilder.DropTable(
                name: "tb_Module");

            migrationBuilder.DropIndex(
                name: "IX_tb_Permission_module_id",
                table: "tb_Permission");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "tb_Permission");
        }
    }
}

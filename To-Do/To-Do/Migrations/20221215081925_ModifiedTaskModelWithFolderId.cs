using Microsoft.EntityFrameworkCore.Migrations;

namespace To_Do.Migrations
{
    public partial class ModifiedTaskModelWithFolderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "TodoTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_FolderId",
                table: "TodoTasks",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks");

            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_FolderId",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "TodoTasks");
        }
    }
}

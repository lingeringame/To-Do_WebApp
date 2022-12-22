using Microsoft.EntityFrameworkCore.Migrations;

namespace To_Do.Migrations
{
    public partial class CustomUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "TodoTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "TodoTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_Folders_FolderId",
                table: "TodoTasks",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace To_Do.Migrations
{
    public partial class AddedIsCompletedPropertyToTaskModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isImportant",
                table: "TodoTasks",
                newName: "IsImportant");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TodoTasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TodoTasks");

            migrationBuilder.RenameColumn(
                name: "IsImportant",
                table: "TodoTasks",
                newName: "isImportant");
        }
    }
}

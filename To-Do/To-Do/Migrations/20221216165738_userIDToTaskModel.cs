using Microsoft.EntityFrameworkCore.Migrations;

namespace To_Do.Migrations
{
    public partial class userIDToTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "TodoTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "TodoTasks");
        }
    }
}

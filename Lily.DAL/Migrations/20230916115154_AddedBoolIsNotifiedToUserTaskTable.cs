using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.DAL.Migrations
{
    public partial class AddedBoolIsNotifiedToUserTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotified",
                table: "UserTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotified",
                table: "UserTasks");
        }
    }
}

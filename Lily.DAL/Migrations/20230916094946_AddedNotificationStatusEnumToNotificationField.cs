using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.DAL.Migrations
{
    public partial class AddedNotificationStatusEnumToNotificationField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationStatus",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "Notifications");
        }
    }
}

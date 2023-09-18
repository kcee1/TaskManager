using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.DAL.Migrations
{
    public partial class AddedProjectTableAndNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTask_AspNetUsers_UserId",
                table: "UserTask");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTask_Project_ProjectId",
                table: "UserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTask",
                table: "UserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "UserTask",
                newName: "UserTasks");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_UserTask_UserId",
                table: "UserTasks",
                newName: "IX_UserTasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTask_ProjectId",
                table: "UserTasks",
                newName: "IX_UserTasks_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AspNetUsers_UserId",
                table: "UserTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Projects_ProjectId",
                table: "UserTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AspNetUsers_UserId",
                table: "UserTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Projects_ProjectId",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "UserTask");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_UserId",
                table: "UserTask",
                newName: "IX_UserTask_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_ProjectId",
                table: "UserTask",
                newName: "IX_UserTask_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTask",
                table: "UserTask",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTask_AspNetUsers_UserId",
                table: "UserTask",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTask_Project_ProjectId",
                table: "UserTask",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

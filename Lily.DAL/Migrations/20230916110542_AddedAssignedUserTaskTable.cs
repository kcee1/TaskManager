using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.DAL.Migrations
{
    public partial class AddedAssignedUserTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignedUserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserTaskId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedUserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedUserTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignedUserTasks_UserTasks_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedUserTasks_UserId",
                table: "AssignedUserTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedUserTasks_UserTaskId",
                table: "AssignedUserTasks",
                column: "UserTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedUserTasks");
        }
    }
}

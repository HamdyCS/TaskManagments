using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusAndTypesAndOtpPurposesTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectTaskStatuses_TaskStatusId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskPriorities_TaskPriorityId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_InviteStatusId",
                table: "WorkSpaceInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaceUsers_WorkSpaceRoles_WorkSpaceRoleId",
                table: "WorkSpaceUsers");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "ProjectTaskStatuses");

            migrationBuilder.DropTable(
                name: "TaskPriorities");

            migrationBuilder.DropTable(
                name: "WorkSpaceInviteStatuses");

            migrationBuilder.DropTable(
                name: "WorkSpaceRoles");

            migrationBuilder.DropIndex(
                name: "IX_WorkSpaceUsers_WorkSpaceRoleId",
                table: "WorkSpaceUsers");

            migrationBuilder.DropIndex(
                name: "IX_WorkSpaceInvites_InviteStatusId",
                table: "WorkSpaceInvites");

            migrationBuilder.DropIndex(
                name: "IX_WorkSpaceInvites_WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskPriorityId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskStatusId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites");

            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceRole",
                table: "WorkSpaceUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceInviteStatus",
                table: "WorkSpaceInvites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskPriority",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskStatus",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkSpaceRole",
                table: "WorkSpaceUsers");

            migrationBuilder.DropColumn(
                name: "WorkSpaceInviteStatus",
                table: "WorkSpaceInvites");

            migrationBuilder.DropColumn(
                name: "TaskPriority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Notifications");

            migrationBuilder.AddColumn<short>(
                name: "WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskPriorities",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkSpaceInviteStatuses",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpaceInviteStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkSpaceRoles",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpaceRoles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "TaskAssigned" },
                    { (short)2, "CommentAdded" },
                    { (short)3, "DueDateReminder" },
                    { (short)4, "TaskDeleted" },
                    { (short)5, "WorkSpaceInvite" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTaskStatuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { (short)1, null, "Backlog" },
                    { (short)2, null, "Todo" },
                    { (short)3, null, "InProgress" },
                    { (short)4, null, "Review" },
                    { (short)5, null, "Done" }
                });

            migrationBuilder.InsertData(
                table: "TaskPriorities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { (short)1, null, "Low" },
                    { (short)2, null, "Medium" },
                    { (short)3, null, "High" },
                    { (short)4, null, "Critical" }
                });

            migrationBuilder.InsertData(
                table: "WorkSpaceInviteStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Pending" },
                    { (short)2, "Accepted" },
                    { (short)3, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "WorkSpaceRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "WorkspaceOwner" },
                    { (short)2, "ProjectManager" },
                    { (short)3, "Member" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceUsers_WorkSpaceRoleId",
                table: "WorkSpaceUsers",
                column: "WorkSpaceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_InviteStatusId",
                table: "WorkSpaceInvites",
                column: "InviteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites",
                column: "WorkSpaceInviteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskPriorityId",
                table: "Tasks",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskStatusId",
                table: "Tasks",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectTaskStatuses_TaskStatusId",
                table: "Tasks",
                column: "TaskStatusId",
                principalTable: "ProjectTaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskPriorities_TaskPriorityId",
                table: "Tasks",
                column: "TaskPriorityId",
                principalTable: "TaskPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_InviteStatusId",
                table: "WorkSpaceInvites",
                column: "InviteStatusId",
                principalTable: "WorkSpaceInviteStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites",
                column: "WorkSpaceInviteStatusId",
                principalTable: "WorkSpaceInviteStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaceUsers_WorkSpaceRoles_WorkSpaceRoleId",
                table: "WorkSpaceUsers",
                column: "WorkSpaceRoleId",
                principalTable: "WorkSpaceRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkSpaceInviteStatusesAndWorkSpaceInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tasks_TaskId",
                table: "Notifications");

            migrationBuilder.AlterColumn<long>(
                name: "TaskId",
                table: "Notifications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "WorkSpaceInviteId",
                table: "Notifications",
                type: "bigint",
                nullable: true);

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
                name: "WorkSpaceInvites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkSpaceId = table.Column<long>(type: "bigint", nullable: false),
                    InitedToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvitedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InviteStatusId = table.Column<short>(type: "smallint", nullable: false),
                    WorkSpaceInviteStatusId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpaceInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSpaceInvites_Users_InitedToId",
                        column: x => x.InitedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkSpaceInvites_Users_InvitedById",
                        column: x => x.InvitedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_InviteStatusId",
                        column: x => x.InviteStatusId,
                        principalTable: "WorkSpaceInviteStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSpaceInvites_WorkSpaceInviteStatuses_WorkSpaceInviteStatusId",
                        column: x => x.WorkSpaceInviteStatusId,
                        principalTable: "WorkSpaceInviteStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkSpaceInvites_WorkSpaces_WorkSpaceId",
                        column: x => x.WorkSpaceId,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)4, "TaskDeleted" },
                    { (short)5, "WorkSpaceInvite" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WorkSpaceInviteId",
                table: "Notifications",
                column: "WorkSpaceInviteId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_InitedToId",
                table: "WorkSpaceInvites",
                column: "InitedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_InvitedById",
                table: "WorkSpaceInvites",
                column: "InvitedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_InviteStatusId",
                table: "WorkSpaceInvites",
                column: "InviteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_WorkSpaceId_InvitedById",
                table: "WorkSpaceInvites",
                columns: new[] { "WorkSpaceId", "InvitedById" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaceInvites_WorkSpaceInviteStatusId",
                table: "WorkSpaceInvites",
                column: "WorkSpaceInviteStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tasks_TaskId",
                table: "Notifications",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_WorkSpaceInvites_WorkSpaceInviteId",
                table: "Notifications",
                column: "WorkSpaceInviteId",
                principalTable: "WorkSpaceInvites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tasks_TaskId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_WorkSpaceInvites_WorkSpaceInviteId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "WorkSpaceInvites");

            migrationBuilder.DropTable(
                name: "WorkSpaceInviteStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_WorkSpaceInviteId",
                table: "Notifications");

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: (short)5);

            migrationBuilder.DropColumn(
                name: "WorkSpaceInviteId",
                table: "Notifications");

            migrationBuilder.AlterColumn<long>(
                name: "TaskId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tasks_TaskId",
                table: "Notifications",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

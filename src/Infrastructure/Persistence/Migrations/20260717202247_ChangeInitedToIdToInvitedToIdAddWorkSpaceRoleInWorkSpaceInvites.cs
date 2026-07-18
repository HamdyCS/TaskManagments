using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInitedToIdToInvitedToIdAddWorkSpaceRoleInWorkSpaceInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaceInvites_Users_InitedToId",
                table: "WorkSpaceInvites");

            migrationBuilder.RenameColumn(
                name: "InitedToId",
                table: "WorkSpaceInvites",
                newName: "InvitedToId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSpaceInvites_InitedToId",
                table: "WorkSpaceInvites",
                newName: "IX_WorkSpaceInvites_InvitedToId");

            migrationBuilder.AddColumn<int>(
                name: "WorkSpaceRole",
                table: "WorkSpaceInvites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaceInvites_Users_InvitedToId",
                table: "WorkSpaceInvites",
                column: "InvitedToId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaceInvites_Users_InvitedToId",
                table: "WorkSpaceInvites");

            migrationBuilder.DropColumn(
                name: "WorkSpaceRole",
                table: "WorkSpaceInvites");

            migrationBuilder.RenameColumn(
                name: "InvitedToId",
                table: "WorkSpaceInvites",
                newName: "InitedToId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSpaceInvites_InvitedToId",
                table: "WorkSpaceInvites",
                newName: "IX_WorkSpaceInvites_InitedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaceInvites_Users_InitedToId",
                table: "WorkSpaceInvites",
                column: "InitedToId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

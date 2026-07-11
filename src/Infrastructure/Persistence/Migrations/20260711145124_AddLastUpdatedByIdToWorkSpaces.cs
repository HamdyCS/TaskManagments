using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastUpdatedByIdToWorkSpaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "WorkSpaces",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaces_LastUpdatedById",
                table: "WorkSpaces",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaces_Users_LastUpdatedById",
                table: "WorkSpaces",
                column: "LastUpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaces_Users_LastUpdatedById",
                table: "WorkSpaces");

            migrationBuilder.DropIndex(
                name: "IX_WorkSpaces_LastUpdatedById",
                table: "WorkSpaces");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "WorkSpaces");
        }
    }
}

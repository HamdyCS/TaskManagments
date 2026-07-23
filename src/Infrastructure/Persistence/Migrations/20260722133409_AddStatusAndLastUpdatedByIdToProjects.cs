using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAndLastUpdatedByIdToProjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LastUpdatedById",
                table: "Projects",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_LastUpdatedById",
                table: "Projects",
                column: "LastUpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_LastUpdatedById",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LastUpdatedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");
        }
    }
}

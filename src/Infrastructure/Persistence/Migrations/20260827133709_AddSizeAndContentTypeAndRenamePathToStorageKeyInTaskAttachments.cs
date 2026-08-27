using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeAndContentTypeAndRenamePathToStorageKeyInTaskAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "TaskAttachments",
                newName: "StorageKey");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Size",
                table: "TaskAttachments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "TaskAttachments");

            migrationBuilder.RenameColumn(
                name: "StorageKey",
                table: "TaskAttachments",
                newName: "Path");
        }
    }
}

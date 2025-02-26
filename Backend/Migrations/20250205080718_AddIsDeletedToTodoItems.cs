using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToTodoItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "TodoItems",
                newName: "Time");

            migrationBuilder.AddColumn<bool>(
                name: "IsInRecyclingBin",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInRecyclingBin",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "TodoItems",
                newName: "time");
        }
    }
}

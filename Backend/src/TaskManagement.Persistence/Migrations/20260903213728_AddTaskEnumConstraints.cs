using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskEnumConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Tasks_Priority",
                table: "Tasks",
                sql: "[Priority] IN (1, 2, 3, 4)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tasks_Status",
                table: "Tasks",
                sql: "[Status] IN (1, 2, 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tasks_Priority",
                table: "Tasks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Tasks_Status",
                table: "Tasks");
        }
    }
}

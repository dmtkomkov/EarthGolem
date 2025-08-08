using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameCloseDateEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "Projects",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "Goals",
                newName: "EndDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Projects",
                newName: "CloseDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Goals",
                newName: "CloseDate");
        }
    }
}

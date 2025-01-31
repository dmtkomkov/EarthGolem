using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewStepFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Projects_ProjectId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Steps",
                type: "TEXT",
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Steps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalId",
                table: "Steps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Goals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Steps_CategoryId",
                table: "Steps",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_GoalId",
                table: "Steps",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Projects_ProjectId",
                table: "Goals",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Categories_CategoryId",
                table: "Steps",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Goals_GoalId",
                table: "Steps",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Projects_ProjectId",
                table: "Goals");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Categories_CategoryId",
                table: "Steps");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Goals_GoalId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Steps_CategoryId",
                table: "Steps");

            migrationBuilder.DropIndex(
                name: "IX_Steps_GoalId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "Steps");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Steps",
                type: "TEXT",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Goals",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Projects_ProjectId",
                table: "Goals",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

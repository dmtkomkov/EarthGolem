using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false, collation: "NOCASE"),
                    Label = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    SpentOn = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PaymentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCategories_Label",
                table: "PaymentCategories",
                column: "Label",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCategories_Name",
                table: "PaymentCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CategoryId",
                table: "Payments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SpentOn",
                table: "Payments",
                column: "SpentOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentCategories");
        }
    }
}

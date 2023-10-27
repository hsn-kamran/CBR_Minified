using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CBR_Minified.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyCourses",
                columns: table => new
                {
                    CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumCode = table.Column<string>(type: "text", nullable: false),
                    CharCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Nominal = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    VunitRate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyCourses", x => new { x.CurrencyId, x.Date });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyCourses_CurrencyId_Date",
                table: "CurrencyCourses",
                columns: new[] { "CurrencyId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyCourses");
        }
    }
}

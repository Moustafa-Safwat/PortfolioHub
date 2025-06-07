using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioHub.Users.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Users",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "46223b84-be0f-495b-9361-0f20ccb032a2", "1", "guest", "GUEST" },
                    { "d9fca67b-4cc0-48bb-858f-8704249a8eb8", "2", "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Users",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46223b84-be0f-495b-9361-0f20ccb032a2");

            migrationBuilder.DeleteData(
                schema: "Users",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9fca67b-4cc0-48bb-858f-8704249a8eb8");
        }
    }
}

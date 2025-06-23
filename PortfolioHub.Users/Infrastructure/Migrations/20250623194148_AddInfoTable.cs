using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Users.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Infos",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InfoKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InfoValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Infos_InfoKey",
                schema: "Users",
                table: "Infos",
                column: "InfoKey")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infos",
                schema: "Users");
        }
    }
}

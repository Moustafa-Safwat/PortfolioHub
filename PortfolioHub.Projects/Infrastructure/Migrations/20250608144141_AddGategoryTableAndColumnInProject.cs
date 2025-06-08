using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Projects.Migrations
{
    /// <inheritdoc />
    public partial class AddGategoryTableAndColumnInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "Projects",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                schema: "Projects",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                schema: "Projects",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CategoryId",
                schema: "Projects",
                table: "Projects",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Category_CategoryId",
                schema: "Projects",
                table: "Projects",
                column: "CategoryId",
                principalSchema: "Projects",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Category_CategoryId",
                schema: "Projects",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CategoryId",
                schema: "Projects",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Projects",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                schema: "Projects",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                schema: "Projects",
                table: "Projects");
        }
    }
}

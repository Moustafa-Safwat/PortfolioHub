using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Projects.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeaturedColumnInProjectsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "Projects",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsFeatured",
                schema: "Projects",
                table: "Projects",
                column: "IsFeatured")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_IsFeatured",
                schema: "Projects",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "Projects",
                table: "Projects");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Blogs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileNamePropForBlogBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "blogs",
                table: "BlogPostBlocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "blogs",
                table: "BlogPostBlocks");
        }
    }
}

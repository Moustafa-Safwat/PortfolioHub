using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Blogs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeaturedBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "blogs",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "blogs",
                table: "BlogPosts");
        }
    }
}

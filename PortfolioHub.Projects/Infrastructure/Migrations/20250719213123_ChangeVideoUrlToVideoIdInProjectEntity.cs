using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Projects.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVideoUrlToVideoIdInProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                schema: "Projects",
                table: "Projects",
                newName: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoId",
                schema: "Projects",
                table: "Projects",
                newName: "VideoUrl");
        }
    }
}

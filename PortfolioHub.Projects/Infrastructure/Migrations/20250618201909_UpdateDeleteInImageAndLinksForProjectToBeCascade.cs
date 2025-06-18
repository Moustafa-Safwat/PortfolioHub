using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Projects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteInImageAndLinksForProjectToBeCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Projects_ProjectId",
                schema: "Projects",
                table: "Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Projects_ProjectId",
                schema: "Projects",
                table: "Links");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Projects_ProjectId",
                schema: "Projects",
                table: "Galleries",
                column: "ProjectId",
                principalSchema: "Projects",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Projects_ProjectId",
                schema: "Projects",
                table: "Links",
                column: "ProjectId",
                principalSchema: "Projects",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Projects_ProjectId",
                schema: "Projects",
                table: "Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Projects_ProjectId",
                schema: "Projects",
                table: "Links");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Projects_ProjectId",
                schema: "Projects",
                table: "Galleries",
                column: "ProjectId",
                principalSchema: "Projects",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Projects_ProjectId",
                schema: "Projects",
                table: "Links",
                column: "ProjectId",
                principalSchema: "Projects",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}

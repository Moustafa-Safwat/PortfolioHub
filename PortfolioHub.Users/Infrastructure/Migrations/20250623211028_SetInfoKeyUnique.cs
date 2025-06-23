using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Users.Migrations
{
    /// <inheritdoc />
    public partial class SetInfoKeyUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Infos_InfoKey",
                schema: "Users",
                table: "Infos");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_InfoKey",
                schema: "Users",
                table: "Infos",
                column: "InfoKey",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Infos_InfoKey",
                schema: "Users",
                table: "Infos");

            migrationBuilder.CreateIndex(
                name: "IX_Infos_InfoKey",
                schema: "Users",
                table: "Infos",
                column: "InfoKey")
                .Annotation("SqlServer:Clustered", false);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Projects.Migrations
{
    /// <inheritdoc />
    public partial class InitProjectsDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Projects");

            migrationBuilder.CreateTable(
                name: "LinkProviders",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSkills",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Galleries",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galleries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Projects",
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Links",
                schema: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_LinkProviders_LinkProviderId",
                        column: x => x.LinkProviderId,
                        principalSchema: "Projects",
                        principalTable: "LinkProviders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Links_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Projects",
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTechanicalSkills",
                schema: "Projects",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechanicalSkills", x => new { x.ProjectsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_ProjectTechanicalSkills_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalSchema: "Projects",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTechanicalSkills_TechnicalSkills_SkillsId",
                        column: x => x.SkillsId,
                        principalSchema: "Projects",
                        principalTable: "TechnicalSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ProjectId",
                schema: "Projects",
                table: "Galleries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_LinkProviderId",
                schema: "Projects",
                table: "Links",
                column: "LinkProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ProjectId",
                schema: "Projects",
                table: "Links",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechanicalSkills_SkillsId",
                schema: "Projects",
                table: "ProjectTechanicalSkills",
                column: "SkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Galleries",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "Links",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectTechanicalSkills",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "LinkProviders",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "Projects");

            migrationBuilder.DropTable(
                name: "TechnicalSkills",
                schema: "Projects");
        }
    }
}

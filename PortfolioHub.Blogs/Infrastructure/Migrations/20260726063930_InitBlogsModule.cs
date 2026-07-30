using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Blogs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitBlogsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blogs");

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PublishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTags",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostAuthors",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostAuthors_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostBlocks",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlockType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    MimeType = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    CodeTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CodeLanguage = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TextAlign = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostBlocks", x => x.Id);
                    table.CheckConstraint("CK_BlogPostBlocks_Order", "[Order] >= 0");
                    table.ForeignKey(
                        name: "FK_BlogPostBlocks_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostComments",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostComments_BlogPostComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalSchema: "blogs",
                        principalTable: "BlogPostComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogPostComments_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostLikes",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UnLikedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostLikes_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostReferences",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostReferences_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostViews",
                schema: "blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstViewedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastViewAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostViews", x => x.Id);
                    table.CheckConstraint("CK_BlogPostViews_ViewCount", "[ViewCount] >= 1");
                    table.ForeignKey(
                        name: "FK_BlogPostViews_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTagLinks",
                schema: "blogs",
                columns: table => new
                {
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTagLinks", x => new { x.BlogPostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogPostTagLinks_BlogPostTags_TagId",
                        column: x => x.TagId,
                        principalSchema: "blogs",
                        principalTable: "BlogPostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTagLinks_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "blogs",
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostAuthors_BlogPostId_Role_IsDeleted",
                schema: "blogs",
                table: "BlogPostAuthors",
                columns: new[] { "BlogPostId", "Role", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostAuthors_UserId",
                schema: "blogs",
                table: "BlogPostAuthors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostAuthors_BlogPostId_UserId_Active",
                schema: "blogs",
                table: "BlogPostAuthors",
                columns: new[] { "BlogPostId", "UserId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostBlocks_BlogPostId_BlockType",
                schema: "blogs",
                table: "BlogPostBlocks",
                columns: new[] { "BlogPostId", "BlockType" });

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostBlocks_BlogPostId_Order_Active",
                schema: "blogs",
                table: "BlogPostBlocks",
                columns: new[] { "BlogPostId", "Order" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_BlogPostId",
                schema: "blogs",
                table: "BlogPostComments",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_BlogPostId_CreatedAtUtc",
                schema: "blogs",
                table: "BlogPostComments",
                columns: new[] { "BlogPostId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_ParentCommentId",
                schema: "blogs",
                table: "BlogPostComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_UserId",
                schema: "blogs",
                table: "BlogPostComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostLikes_BlogPostId_IsLiked",
                schema: "blogs",
                table: "BlogPostLikes",
                columns: new[] { "BlogPostId", "IsLiked" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostLikes_UserId",
                schema: "blogs",
                table: "BlogPostLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostLikes_BlogPostId_UserId",
                schema: "blogs",
                table: "BlogPostLikes",
                columns: new[] { "BlogPostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostReferences_BlogPostId",
                schema: "blogs",
                table: "BlogPostReferences",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostReferences_BlogPostId_Url_Active",
                schema: "blogs",
                table: "BlogPostReferences",
                columns: new[] { "BlogPostId", "Url" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CreatedAtUtc",
                schema: "blogs",
                table: "BlogPosts",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CreatedBy",
                schema: "blogs",
                table: "BlogPosts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_IsDeleted",
                schema: "blogs",
                table: "BlogPosts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublishedAt",
                schema: "blogs",
                table: "BlogPosts",
                column: "PublishedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Status",
                schema: "blogs",
                table: "BlogPosts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPosts_Slug",
                schema: "blogs",
                table: "BlogPosts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTagLinks_TagId",
                schema: "blogs",
                table: "BlogPostTagLinks",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostTags_Name",
                schema: "blogs",
                table: "BlogPostTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostViews_BlogPostId",
                schema: "blogs",
                table: "BlogPostViews",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostViews_UserId",
                schema: "blogs",
                table: "BlogPostViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_BlogPostViews_BlogPostId_UserId",
                schema: "blogs",
                table: "BlogPostViews",
                columns: new[] { "BlogPostId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostAuthors",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostBlocks",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostComments",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostLikes",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostReferences",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostTagLinks",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostViews",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPostTags",
                schema: "blogs");

            migrationBuilder.DropTable(
                name: "BlogPosts",
                schema: "blogs");
        }
    }
}

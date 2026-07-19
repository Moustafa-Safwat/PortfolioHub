using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioHub.Users.Infrastructure.Migrations
{
    /// <summary>
    /// Converts ASP.NET Core Identity string keys to Guid keys while preserving
    /// existing users, roles, claims, logins, tokens, role assignments and refresh tokens.
    ///
    /// IMPORTANT:
    /// 1. Back up the database before applying this migration.
    /// 2. This migration assumes the standard ASP.NET Core Identity table/index names.
    /// 3. Existing non-Guid string IDs are remapped to new Guid values consistently.
    /// </summary>
    public partial class AddApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // -----------------------------------------------------------------
            // 1. Remove foreign keys that depend on the string identity keys.
            // -----------------------------------------------------------------
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                schema: "Users",
                table: "RefreshTokens");

            // -----------------------------------------------------------------
            // 2. Drop primary keys and indexes that depend on columns whose
            //    types will change from nvarchar(450) to uniqueidentifier.
            // -----------------------------------------------------------------
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "Users",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Users",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Users",
                table: "AspNetUserTokens");

            // Drop dependent indexes only when they exist.
            migrationBuilder.Sql(
                """
                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_AspNetUserClaims_UserId'
                      AND object_id = OBJECT_ID(N'[Users].[AspNetUserClaims]')
                )
                    DROP INDEX [IX_AspNetUserClaims_UserId]
                    ON [Users].[AspNetUserClaims];

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_AspNetUserLogins_UserId'
                      AND object_id = OBJECT_ID(N'[Users].[AspNetUserLogins]')
                )
                    DROP INDEX [IX_AspNetUserLogins_UserId]
                    ON [Users].[AspNetUserLogins];

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_AspNetUserRoles_RoleId'
                      AND object_id = OBJECT_ID(N'[Users].[AspNetUserRoles]')
                )
                    DROP INDEX [IX_AspNetUserRoles_RoleId]
                    ON [Users].[AspNetUserRoles];

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_AspNetRoleClaims_RoleId'
                      AND object_id = OBJECT_ID(N'[Users].[AspNetRoleClaims]')
                )
                    DROP INDEX [IX_AspNetRoleClaims_RoleId]
                    ON [Users].[AspNetRoleClaims];

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = N'IX_RefreshTokens_UserId'
                      AND object_id = OBJECT_ID(N'[Users].[RefreshTokens]')
                )
                    DROP INDEX [IX_RefreshTokens_UserId]
                    ON [Users].[RefreshTokens];
                """);

            // -----------------------------------------------------------------
            // 3. Normalize all existing string IDs to Guid strings.
            //
            //    Valid Guid strings keep their existing values.
            //    Non-Guid strings receive a new Guid, and every related table
            //    is updated using the same mapping.
            // -----------------------------------------------------------------
            migrationBuilder.Sql(
                """
                SET NOCOUNT ON;

                CREATE TABLE #UserIdMap
                (
                    OldId nvarchar(450) NOT NULL PRIMARY KEY,
                    NewId uniqueidentifier NOT NULL
                );

                INSERT INTO #UserIdMap (OldId, NewId)
                SELECT
                    [Id],
                    COALESCE(TRY_CONVERT(uniqueidentifier, [Id]), NEWID())
                FROM [Users].[AspNetUsers];

                CREATE TABLE #RoleIdMap
                (
                    OldId nvarchar(450) NOT NULL PRIMARY KEY,
                    NewId uniqueidentifier NOT NULL
                );

                INSERT INTO #RoleIdMap (OldId, NewId)
                SELECT
                    [Id],
                    COALESCE(TRY_CONVERT(uniqueidentifier, [Id]), NEWID())
                FROM [Users].[AspNetRoles];

                UPDATE target
                SET target.[UserId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUserClaims] AS target
                INNER JOIN #UserIdMap AS map ON target.[UserId] = map.[OldId];

                UPDATE target
                SET target.[UserId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUserLogins] AS target
                INNER JOIN #UserIdMap AS map ON target.[UserId] = map.[OldId];

                UPDATE target
                SET target.[UserId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUserRoles] AS target
                INNER JOIN #UserIdMap AS map ON target.[UserId] = map.[OldId];

                UPDATE target
                SET target.[UserId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUserTokens] AS target
                INNER JOIN #UserIdMap AS map ON target.[UserId] = map.[OldId];

                UPDATE target
                SET target.[UserId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[RefreshTokens] AS target
                INNER JOIN #UserIdMap AS map ON target.[UserId] = map.[OldId];

                UPDATE target
                SET target.[RoleId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUserRoles] AS target
                INNER JOIN #RoleIdMap AS map ON target.[RoleId] = map.[OldId];

                UPDATE target
                SET target.[RoleId] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetRoleClaims] AS target
                INNER JOIN #RoleIdMap AS map ON target.[RoleId] = map.[OldId];

                UPDATE target
                SET target.[Id] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetUsers] AS target
                INNER JOIN #UserIdMap AS map ON target.[Id] = map.[OldId];

                UPDATE target
                SET target.[Id] = CONVERT(nvarchar(36), map.[NewId])
                FROM [Users].[AspNetRoles] AS target
                INNER JOIN #RoleIdMap AS map ON target.[Id] = map.[OldId];

                DROP TABLE #UserIdMap;
                DROP TABLE #RoleIdMap;
                """);

            // -----------------------------------------------------------------
            // 4. Rename the existing users table instead of deleting it.
            // -----------------------------------------------------------------
            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "Users",
                newName: "ApplicationUsers",
                newSchema: "Users");

            // -----------------------------------------------------------------
            // 5. Convert all Identity key columns to Guid.
            // -----------------------------------------------------------------
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Users",
                table: "ApplicationUsers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "Users",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "Users",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            // -----------------------------------------------------------------
            // 6. Add ApplicationUser-specific columns to preserved user rows.
            // -----------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Users",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Users",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                schema: "Users",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Users",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Users",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true);

            // -----------------------------------------------------------------
            // 7. Recreate primary keys and Identity indexes.
            // -----------------------------------------------------------------
            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                schema: "Users",
                table: "ApplicationUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Users",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Users",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Users",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "Users",
                table: "RefreshTokens",
                column: "UserId");

            // -----------------------------------------------------------------
            // 8. Create the new one-to-one profile/security tables.
            // -----------------------------------------------------------------
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSecurity",
                schema: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPasswordChangedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSystemUser = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsLockedByAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AdminLockReason = table.Column<string>(
                        type: "nvarchar(1000)",
                        maxLength: 1000,
                        nullable: false,
                        defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecurity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecurity_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                schema: "Users",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSecurity_UserId",
                schema: "Users",
                table: "UserSecurity",
                column: "UserId",
                unique: true);

            // -----------------------------------------------------------------
            // 9. Restore all foreign keys using Guid columns.
            // -----------------------------------------------------------------
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "Users",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "Users",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_UserId",
                schema: "Users",
                table: "RefreshTokens",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_ApplicationUsers_UserId",
                schema: "Users",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_ApplicationUsers_UserId",
                schema: "Users",
                table: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "Users");

            migrationBuilder.DropTable(
                name: "UserSecurity",
                schema: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Users",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Users",
                table: "AspNetUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Users",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Users",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "Users",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Users",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Users",
                table: "ApplicationUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Users",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Users",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "Users",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                schema: "Users",
                newName: "AspNetUsers",
                newSchema: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "Users",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Users",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Users",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Users",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "Users",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "Users",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Users",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Users",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "Users",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                schema: "Users",
                table: "RefreshTokens",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
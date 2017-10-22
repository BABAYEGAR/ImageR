using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_PhotographerCategories_PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_Roles_RoleId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptions_RoleId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "NumberOfImagesDownloaded",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserSubscriptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<bool>(
                name: "Contracts",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CompetitionId",
                table: "CompetitionVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccessKeys",
                columns: table => new
                {
                    AppUserAccessKeyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountActivationAccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    PasswordAccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessKeys", x => x.AppUserAccessKeyId);
                    table.ForeignKey(
                        name: "FK_AccessKeys_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionCategories",
                columns: table => new
                {
                    CompetitionCategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    PhotographerCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionCategories", x => x.CompetitionCategoryId);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCategories_PhotographerCategories_PhotographerCategoryId",
                        column: x => x.PhotographerCategoryId,
                        principalTable: "PhotographerCategories",
                        principalColumn: "PhotographerCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionUploads",
                columns: table => new
                {
                    CompetitionUploadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompetitionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionUploads", x => x.CompetitionUploadId);
                    table.ForeignKey(
                        name: "FK_CompetitionUploads_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionUploads_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotographerCategoryMappings",
                columns: table => new
                {
                    PhotographerCategoryMappingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    PhotographerCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotographerCategoryMappings", x => x.PhotographerCategoryMappingId);
                    table.ForeignKey(
                        name: "FK_PhotographerCategoryMappings_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotographerCategoryMappings_PhotographerCategories_PhotographerCategoryId",
                        column: x => x.PhotographerCategoryId,
                        principalTable: "PhotographerCategories",
                        principalColumn: "PhotographerCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVote_CompetitionId",
                table: "CompetitionVote",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessKeys_AppUserId",
                table: "AccessKeys",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_CompetitionId",
                table: "CompetitionCategories",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_PhotographerCategoryId",
                table: "CompetitionCategories",
                column: "PhotographerCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_AppUserId",
                table: "CompetitionUploads",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_CompetitionId",
                table: "CompetitionUploads",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerCategoryMappings_AppUserId",
                table: "PhotographerCategoryMappings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerCategoryMappings_PhotographerCategoryId",
                table: "PhotographerCategoryMappings",
                column: "PhotographerCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionVote_Competition_CompetitionId",
                table: "CompetitionVote",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionVote_Competition_CompetitionId",
                table: "CompetitionVote");

            migrationBuilder.DropTable(
                name: "AccessKeys");

            migrationBuilder.DropTable(
                name: "CompetitionCategories");

            migrationBuilder.DropTable(
                name: "CompetitionUploads");

            migrationBuilder.DropTable(
                name: "PhotographerCategoryMappings");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionVote_CompetitionId",
                table: "CompetitionVote");

            migrationBuilder.DropColumn(
                name: "Contracts",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "CompetitionVote");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "AppUsers");

            migrationBuilder.AlterColumn<long>(
                name: "Status",
                table: "UserSubscriptions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NumberOfImagesDownloaded",
                table: "UserSubscriptions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "UserSubscriptions",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PhotographerCategoryId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_RoleId",
                table: "UserSubscriptions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_PhotographerCategoryId",
                table: "AppUsers",
                column: "PhotographerCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_PhotographerCategories_PhotographerCategoryId",
                table: "AppUsers",
                column: "PhotographerCategoryId",
                principalTable: "PhotographerCategories",
                principalColumn: "PhotographerCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_Roles_RoleId",
                table: "UserSubscriptions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

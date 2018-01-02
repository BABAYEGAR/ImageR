using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CamerackStudio.Migrations
{
    public partial class Migrate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropTable(
                name: "CompetitionCategories");

            migrationBuilder.DropColumn(
                name: "ClearityRating",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "ConceptRating",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "QualityRating",
                table: "ImageCompetitionRatings");

            migrationBuilder.AlterColumn<long>(
                name: "TotalRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TimeDeliveryRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "DescriptionRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CompetitionUploadId",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "AcceptanceRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings",
                column: "CompetitionUploadId",
                principalTable: "CompetitionUploads",
                principalColumn: "CompetitionUploadId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings");

            migrationBuilder.AlterColumn<long>(
                name: "TotalRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TimeDeliveryRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DescriptionRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompetitionUploadId",
                table: "ImageCompetitionRatings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AcceptanceRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ClearityRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ConceptRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "QualityRating",
                table: "ImageCompetitionRatings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CompetitionCategories",
                columns: table => new
                {
                    CompetitionCategoryId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    PhotographerCategoryId = table.Column<long>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_CompetitionId",
                table: "CompetitionCategories",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCategories_PhotographerCategoryId",
                table: "CompetitionCategories",
                column: "PhotographerCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings",
                column: "CompetitionUploadId",
                principalTable: "CompetitionUploads",
                principalColumn: "CompetitionUploadId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

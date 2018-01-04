using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CamerackStudio.Migrations
{
    public partial class Migrate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropTable(
                name: "CompetitionVote");

            migrationBuilder.DropTable(
                name: "CompetitionUploads");

            migrationBuilder.DropIndex(
                name: "IX_ImageCompetitionRatings_CompetitionUploadId",
                table: "ImageCompetitionRatings");

            migrationBuilder.RenameColumn(
                name: "CompetitionUploadId",
                table: "ImageCompetitionRatings",
                newName: "CompetitionId");

            migrationBuilder.AddColumn<long>(
                name: "AppUserId",
                table: "ImageCompetitionRatings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ImageCompetitionRatings");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "ImageCompetitionRatings",
                newName: "CompetitionUploadId");

            migrationBuilder.CreateTable(
                name: "CompetitionUploads",
                columns: table => new
                {
                    CompetitionUploadId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(nullable: true),
                    CameraId = table.Column<long>(nullable: true),
                    CompetitionId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Inspiration = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    LocationId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Vote = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionUploads", x => x.CompetitionUploadId);
                    table.ForeignKey(
                        name: "FK_CompetitionUploads_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "CameraId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionUploads_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionUploads_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionVote",
                columns: table => new
                {
                    CompetitionVoteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(nullable: true),
                    CompetitionId = table.Column<long>(nullable: true),
                    CompetitionUploadId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    OwnerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionVote", x => x.CompetitionVoteId);
                    table.ForeignKey(
                        name: "FK_CompetitionVote_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionVote_CompetitionUploads_CompetitionUploadId",
                        column: x => x.CompetitionUploadId,
                        principalTable: "CompetitionUploads",
                        principalColumn: "CompetitionUploadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageCompetitionRatings_CompetitionUploadId",
                table: "ImageCompetitionRatings",
                column: "CompetitionUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_CameraId",
                table: "CompetitionUploads",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_CompetitionId",
                table: "CompetitionUploads",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_LocationId",
                table: "CompetitionUploads",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVote_CompetitionId",
                table: "CompetitionVote",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVote_CompetitionUploadId",
                table: "CompetitionVote",
                column: "CompetitionUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCompetitionRatings_CompetitionUploads_CompetitionUploadId",
                table: "ImageCompetitionRatings",
                column: "CompetitionUploadId",
                principalTable: "CompetitionUploads",
                principalColumn: "CompetitionUploadId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

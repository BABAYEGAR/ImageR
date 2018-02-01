using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CamerackStudio.Migrations
{
    public partial class Migrate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCompetitionRatings");

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    AdvertisementId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdClick = table.Column<long>(nullable: true),
                    Client = table.Column<string>(nullable: false),
                    ClientEmail = table.Column<string>(nullable: false),
                    ClientPhoneNumber = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PageCategory = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.AdvertisementId);
                });

            migrationBuilder.CreateTable(
                name: "HeaderImages",
                columns: table => new
                {
                    HeaderImageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PageCategory = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderImages", x => x.HeaderImageId);
                });

            migrationBuilder.CreateTable(
                name: "SliderImages",
                columns: table => new
                {
                    SliderImageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    MainHeaderText = table.Column<string>(nullable: false),
                    SliderName = table.Column<string>(nullable: false),
                    Style = table.Column<string>(nullable: true),
                    SubHeaderText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SliderImages", x => x.SliderImageId);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndConditions",
                columns: table => new
                {
                    TermAndConditionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditions", x => x.TermAndConditionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "HeaderImages");

            migrationBuilder.DropTable(
                name: "SliderImages");

            migrationBuilder.DropTable(
                name: "TermsAndConditions");

            migrationBuilder.CreateTable(
                name: "ImageCompetitionRatings",
                columns: table => new
                {
                    ImageCompetitionRatingId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcceptanceRating = table.Column<long>(nullable: true),
                    AppUserId = table.Column<long>(nullable: true),
                    CompetitionId = table.Column<long>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    DescriptionRating = table.Column<long>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    TagsRating = table.Column<long>(nullable: true),
                    TimeDeliveryRating = table.Column<long>(nullable: true),
                    TotalRating = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCompetitionRatings", x => x.ImageCompetitionRatingId);
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisLike",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "AppUsers");

            migrationBuilder.AlterColumn<long>(
                name: "TimeDeliveryRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "QualityRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DescriptionRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ConceptRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ClearityRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AcceptanceRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ImageCompetitionRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "ImageCompetitionRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedBy",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalRating",
                table: "ImageCompetitionRatings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Vote",
                table: "CompetitionUploads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceRating",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "ImageCompetitionRatings");

            migrationBuilder.DropColumn(
                name: "Vote",
                table: "CompetitionUploads");

            migrationBuilder.AlterColumn<long>(
                name: "TimeDeliveryRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "QualityRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DescriptionRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ConceptRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ClearityRating",
                table: "ImageCompetitionRatings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "DisLike",
                table: "CompetitionUploads",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Like",
                table: "CompetitionUploads",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AveragePrice",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "AppUsers",
                nullable: true);
        }
    }
}

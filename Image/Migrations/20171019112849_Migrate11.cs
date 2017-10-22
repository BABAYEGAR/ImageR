using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Competition",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ImagePriority",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ImageUploadNumber",
                table: "Packages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "ImageComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ImageComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "ImageComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedBy",
                table: "ImageComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundPicture",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Competition",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ImagePriority",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ImageUploadNumber",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "BackgroundPicture",
                table: "AppUsers");
        }
    }
}

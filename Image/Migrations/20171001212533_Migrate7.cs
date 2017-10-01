using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Camera",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Images");

            migrationBuilder.AddColumn<bool>(
                name: "ManageCameras",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ManagePhotographerCategory",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CameraId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AppUserId",
                table: "ImageComments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AppUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PhotographerCategoryId",
                table: "AppUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    CameraId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.CameraId);
                });

            migrationBuilder.CreateTable(
                name: "PhotographerCategories",
                columns: table => new
                {
                    PhotographerCategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotographerCategories", x => x.PhotographerCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CameraId",
                table: "Images",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageComments_AppUserId",
                table: "ImageComments",
                column: "AppUserId");

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
                name: "FK_ImageComments_AppUsers_AppUserId",
                table: "ImageComments",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cameras_CameraId",
                table: "Images",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "CameraId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_PhotographerCategories_PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageComments_AppUsers_AppUserId",
                table: "ImageComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cameras_CameraId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "PhotographerCategories");

            migrationBuilder.DropIndex(
                name: "IX_Images_CameraId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_ImageComments_AppUserId",
                table: "ImageComments");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ManageCameras",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ManagePhotographerCategory",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CameraId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ImageComments");

            migrationBuilder.DropColumn(
                name: "Biography",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PhotographerCategoryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "AppUsers");

            migrationBuilder.AddColumn<string>(
                name: "Camera",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Images",
                nullable: true);
        }
    }
}

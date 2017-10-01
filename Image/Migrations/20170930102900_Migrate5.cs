using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImageCategories_ImageCategoryId",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SellingPrice",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ImageCategoryId",
                table: "Images",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageSubCategoryId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageSubCategoryId",
                table: "Images",
                column: "ImageSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImageCategories_ImageCategoryId",
                table: "Images",
                column: "ImageCategoryId",
                principalTable: "ImageCategories",
                principalColumn: "ImageCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImageSubCategories_ImageSubCategoryId",
                table: "Images",
                column: "ImageSubCategoryId",
                principalTable: "ImageSubCategories",
                principalColumn: "ImageSubCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImageCategories_ImageCategoryId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImageSubCategories_ImageSubCategoryId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ImageSubCategoryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageSubCategoryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SellingPrice",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ImageCategoryId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImageCategories_ImageCategoryId",
                table: "Images",
                column: "ImageCategoryId",
                principalTable: "ImageCategories",
                principalColumn: "ImageCategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

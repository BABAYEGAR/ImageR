using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUploadNumber",
                table: "Packages");

            migrationBuilder.AddColumn<bool>(
                name: "Packaging",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Packaging",
                table: "Packages");

            migrationBuilder.AddColumn<long>(
                name: "ImageUploadNumber",
                table: "Packages",
                nullable: true);
        }
    }
}

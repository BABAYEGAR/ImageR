using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Packages");

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "PackageItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "PackageItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastModified",
                table: "PackageItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedBy",
                table: "PackageItem",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Competition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PackageItem");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "PackageItem");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "PackageItem");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PackageItem");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Competition");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Packages",
                nullable: false,
                defaultValue: "");
        }
    }
}

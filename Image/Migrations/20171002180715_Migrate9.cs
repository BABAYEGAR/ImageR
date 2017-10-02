using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Images");

            migrationBuilder.AddColumn<bool>(
                name: "ManageLocations",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "AppUsers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_LocationId",
                table: "Images",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Images_LocationId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ManageLocations",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Images",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

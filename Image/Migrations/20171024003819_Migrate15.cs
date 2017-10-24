using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CameraId",
                table: "CompetitionUploads",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspiration",
                table: "CompetitionUploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "CompetitionUploads",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CompetitionUploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_CameraId",
                table: "CompetitionUploads",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_LocationId",
                table: "CompetitionUploads",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionUploads_Cameras_CameraId",
                table: "CompetitionUploads",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "CameraId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionUploads_Locations_LocationId",
                table: "CompetitionUploads",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionUploads_Cameras_CameraId",
                table: "CompetitionUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionUploads_Locations_LocationId",
                table: "CompetitionUploads");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionUploads_CameraId",
                table: "CompetitionUploads");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionUploads_LocationId",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "CameraId",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "Inspiration",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "CompetitionUploads");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CompetitionUploads");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CamerackStudio.Migrations
{
    public partial class Migrate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Images",
                newName: "CompetitionId");

            migrationBuilder.AddColumn<string>(
                name: "UploadCategory",
                table: "Images",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CompetitionId",
                table: "Images",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Competition_CompetitionId",
                table: "Images",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Competition_CompetitionId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CompetitionId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UploadCategory",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "Images",
                newName: "Rating");
        }
    }
}

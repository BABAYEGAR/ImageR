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
                name: "Votes",
                table: "CompetitionVote");

            migrationBuilder.AddColumn<long>(
                name: "CompetitionUploadId",
                table: "CompetitionVote",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "CompetitionVote",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVote_CompetitionUploadId",
                table: "CompetitionVote",
                column: "CompetitionUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionVote_CompetitionUploads_CompetitionUploadId",
                table: "CompetitionVote",
                column: "CompetitionUploadId",
                principalTable: "CompetitionUploads",
                principalColumn: "CompetitionUploadId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionVote_CompetitionUploads_CompetitionUploadId",
                table: "CompetitionVote");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionVote_CompetitionUploadId",
                table: "CompetitionVote");

            migrationBuilder.DropColumn(
                name: "CompetitionUploadId",
                table: "CompetitionVote");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CompetitionVote");

            migrationBuilder.AddColumn<long>(
                name: "Votes",
                table: "CompetitionVote",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

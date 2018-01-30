using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CamerackStudio.Migrations
{
    public partial class Migrate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Competition_CompetitionId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Competition");

            migrationBuilder.DropIndex(
                name: "IX_Images_CompetitionId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompetitionId",
                table: "Images",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competition",
                columns: table => new
                {
                    CompetitionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Theme = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competition", x => x.CompetitionId);
                });

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
    }
}

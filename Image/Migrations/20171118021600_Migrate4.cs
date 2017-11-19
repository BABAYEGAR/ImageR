using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageItem");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Packages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<long>(nullable: false),
                    Competition = table.Column<bool>(nullable: false),
                    Contracts = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImagePriority = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Packaging = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "PackageItem",
                columns: table => new
                {
                    PackageItemId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PackageId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageItem", x => x.PackageItemId);
                    table.ForeignKey(
                        name: "FK_PackageItem_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    UserSubscriptionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<long>(nullable: true),
                    AppUserId = table.Column<long>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    MonthLength = table.Column<long>(nullable: true),
                    PackageId = table.Column<long>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.UserSubscriptionId);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageItem_PackageId",
                table: "PackageItem",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_PackageId",
                table: "UserSubscriptions",
                column: "PackageId");
        }
    }
}

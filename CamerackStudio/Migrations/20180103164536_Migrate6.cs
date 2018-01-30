using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CamerackStudio.Migrations
{
    public partial class Migrate6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBanks_Banks_BankId",
                table: "UserBanks");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "SystemNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserBanks_BankId",
                table: "UserBanks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "SystemNotifications",
                columns: table => new
                {
                    SystemNotificationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    ControllerId = table.Column<long>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Read = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemNotifications", x => x.SystemNotificationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBanks_BankId",
                table: "UserBanks",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanks_Banks_BankId",
                table: "UserBanks",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

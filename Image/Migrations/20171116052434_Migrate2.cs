using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBanks_Bank_BankId",
                table: "UserBanks");

            migrationBuilder.AlterColumn<long>(
                name: "BankId",
                table: "UserBanks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "UserBanks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanks_Bank_BankId",
                table: "UserBanks",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBanks_Bank_BankId",
                table: "UserBanks");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "UserBanks");

            migrationBuilder.AlterColumn<long>(
                name: "BankId",
                table: "UserBanks",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanks_Bank_BankId",
                table: "UserBanks",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

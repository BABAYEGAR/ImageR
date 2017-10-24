using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemNotification_AppUsers_AppUserId",
                table: "SystemNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemNotification",
                table: "SystemNotification");

            migrationBuilder.RenameTable(
                name: "SystemNotification",
                newName: "SystemNotifications");

            migrationBuilder.RenameIndex(
                name: "IX_SystemNotification_AppUserId",
                table: "SystemNotifications",
                newName: "IX_SystemNotifications_AppUserId");

            migrationBuilder.AddColumn<bool>(
                name: "ParticipateCompetition",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemNotifications",
                table: "SystemNotifications",
                column: "SystemNotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemNotifications_AppUsers_AppUserId",
                table: "SystemNotifications",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemNotifications_AppUsers_AppUserId",
                table: "SystemNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemNotifications",
                table: "SystemNotifications");

            migrationBuilder.DropColumn(
                name: "ParticipateCompetition",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "SystemNotifications",
                newName: "SystemNotification");

            migrationBuilder.RenameIndex(
                name: "IX_SystemNotifications_AppUserId",
                table: "SystemNotification",
                newName: "IX_SystemNotification_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemNotification",
                table: "SystemNotification",
                column: "SystemNotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemNotification_AppUsers_AppUserId",
                table: "SystemNotification",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

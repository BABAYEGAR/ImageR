using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessKeys_AppUsers_AppUserId",
                table: "AccessKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AppUsers_AppUserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Competition_AppUsers_AppUserId",
                table: "Competition");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionUploads_AppUsers_AppUserId",
                table: "CompetitionUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionVote_AppUsers_AppUserId",
                table: "CompetitionVote");

            migrationBuilder.DropForeignKey(
                name: "FK_ImageComments_AppUsers_AppUserId",
                table: "ImageComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AppUsers_AppUserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AppUsers_AppUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotographerCategoryMappings_AppUsers_AppUserId",
                table: "PhotographerCategoryMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemNotifications_AppUsers_AppUserId",
                table: "SystemNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_AppUsers_AppUserId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptions_AppUserId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_SystemNotifications_AppUserId",
                table: "SystemNotifications");

            migrationBuilder.DropIndex(
                name: "IX_PhotographerCategoryMappings_AppUserId",
                table: "PhotographerCategoryMappings");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Images_AppUserId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_ImageComments_AppUserId",
                table: "ImageComments");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionVote_AppUserId",
                table: "CompetitionVote");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionUploads_AppUserId",
                table: "CompetitionUploads");

            migrationBuilder.DropIndex(
                name: "IX_Competition_AppUserId",
                table: "Competition");

            migrationBuilder.DropIndex(
                name: "IX_Carts_AppUserId",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUser");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_RoleId",
                table: "AppUser",
                newName: "IX_AppUser_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessKeys_AppUser_AppUserId",
                table: "AccessKeys",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Roles_RoleId",
                table: "AppUser",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessKeys_AppUser_AppUserId",
                table: "AccessKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_Roles_RoleId",
                table: "AppUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AppUsers");

            migrationBuilder.RenameIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUsers",
                newName: "IX_AppUsers_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_AppUserId",
                table: "UserSubscriptions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemNotifications_AppUserId",
                table: "SystemNotifications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotographerCategoryMappings_AppUserId",
                table: "PhotographerCategoryMappings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AppUserId",
                table: "Images",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageComments_AppUserId",
                table: "ImageComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionVote_AppUserId",
                table: "CompetitionVote",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionUploads_AppUserId",
                table: "CompetitionUploads",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Competition_AppUserId",
                table: "Competition",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_AppUserId",
                table: "Carts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessKeys_AppUsers_AppUserId",
                table: "AccessKeys",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Roles_RoleId",
                table: "AppUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AppUsers_AppUserId",
                table: "Carts",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competition_AppUsers_AppUserId",
                table: "Competition",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionUploads_AppUsers_AppUserId",
                table: "CompetitionUploads",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionVote_AppUsers_AppUserId",
                table: "CompetitionVote",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageComments_AppUsers_AppUserId",
                table: "ImageComments",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AppUsers_AppUserId",
                table: "Images",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AppUsers_AppUserId",
                table: "Orders",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotographerCategoryMappings_AppUsers_AppUserId",
                table: "PhotographerCategoryMappings",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemNotifications_AppUsers_AppUserId",
                table: "SystemNotifications",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_AppUsers_AppUserId",
                table: "UserSubscriptions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Image.Migrations
{
    public partial class Migrate10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUrls",
                columns: table => new
                {
                    ApiUrlId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangePasswordrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditProfileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FetchUsersUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForgotPasswordLinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    RegisterUsersUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenancyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUrls", x => x.ApiUrlId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUrls");
        }
    }
}

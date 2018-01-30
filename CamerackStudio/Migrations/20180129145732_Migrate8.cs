using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CamerackStudio.Migrations
{
    public partial class Migrate8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Height",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Width",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Images");
        }
    }
}

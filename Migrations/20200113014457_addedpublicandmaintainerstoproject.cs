using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class addedpublicandmaintainerstoproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Projects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId1",
                table: "AspNetUsers",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId1",
                table: "AspNetUsers",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "AspNetUsers");
        }
    }
}

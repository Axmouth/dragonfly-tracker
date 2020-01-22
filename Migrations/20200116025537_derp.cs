using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class derp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjectId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "testu",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "Open",
                table: "Issues",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProjectAdmin",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    AdminId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAdmin", x => new { x.ProjectId, x.AdminId });
                    table.ForeignKey(
                        name: "FK_ProjectAdmin_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAdmin_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaintainer",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    MaintainerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaintainer", x => new { x.ProjectId, x.MaintainerId });
                    table.ForeignKey(
                        name: "FK_ProjectMaintainer_AspNetUsers_MaintainerId",
                        column: x => x.MaintainerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMaintainer_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAdmin_AdminId",
                table: "ProjectAdmin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaintainer_MaintainerId",
                table: "ProjectMaintainer",
                column: "MaintainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAdmin");

            migrationBuilder.DropTable(
                name: "ProjectMaintainer");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "Issues");

            migrationBuilder.AddColumn<string>(
                name: "testu",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId1",
                table: "AspNetUsers",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projects_ProjectId1",
                table: "AspNetUsers",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

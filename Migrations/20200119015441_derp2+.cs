using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class derp2plus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueTypes_Issues_IssueId",
                table: "IssueTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAdmin_AspNetUsers_AdminId",
                table: "ProjectAdmin");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAdmin_Projects_ProjectId",
                table: "ProjectAdmin");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMaintainer_AspNetUsers_MaintainerId",
                table: "ProjectMaintainer");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMaintainer_Projects_ProjectId",
                table: "ProjectMaintainer");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_IssueTypes_IssueId",
                table: "IssueTypes");

            migrationBuilder.DropIndex(
                name: "IX_Issues_CompanyId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMaintainer",
                table: "ProjectMaintainer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectAdmin",
                table: "ProjectAdmin");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "IssueTypes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "ProjectMaintainer",
                newName: "ProjectMaintainers");

            migrationBuilder.RenameTable(
                name: "ProjectAdmin",
                newName: "ProjectAdmins");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMaintainer_MaintainerId",
                table: "ProjectMaintainers",
                newName: "IX_ProjectMaintainers_MaintainerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectAdmin_AdminId",
                table: "ProjectAdmins",
                newName: "IX_ProjectAdmins_AdminId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Projects",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Issues",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "IssuePosts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "IssuePosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMaintainers",
                table: "ProjectMaintainers",
                columns: new[] { "ProjectId", "MaintainerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectAdmins",
                table: "ProjectAdmins",
                columns: new[] { "ProjectId", "AdminId" });

            migrationBuilder.CreateTable(
                name: "IssueIssueTypes",
                columns: table => new
                {
                    IssueId = table.Column<Guid>(nullable: false),
                    IssueTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueIssueTypes", x => new { x.IssueId, x.IssueTypeId });
                    table.ForeignKey(
                        name: "FK_IssueIssueTypes_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueIssueTypes_IssueTypes_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "IssueTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_OrganizationId",
                table: "Issues",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueIssueTypes_IssueTypeId",
                table: "IssueIssueTypes",
                column: "IssueTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAdmins_AspNetUsers_AdminId",
                table: "ProjectAdmins",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAdmins_Projects_ProjectId",
                table: "ProjectAdmins",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMaintainers_AspNetUsers_MaintainerId",
                table: "ProjectMaintainers",
                column: "MaintainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMaintainers_Projects_ProjectId",
                table: "ProjectMaintainers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAdmins_AspNetUsers_AdminId",
                table: "ProjectAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectAdmins_Projects_ProjectId",
                table: "ProjectAdmins");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMaintainers_AspNetUsers_MaintainerId",
                table: "ProjectMaintainers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMaintainers_Projects_ProjectId",
                table: "ProjectMaintainers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "IssueIssueTypes");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Issues_OrganizationId",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMaintainers",
                table: "ProjectMaintainers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectAdmins",
                table: "ProjectAdmins");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "IssuePosts");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "IssuePosts");

            migrationBuilder.RenameTable(
                name: "ProjectMaintainers",
                newName: "ProjectMaintainer");

            migrationBuilder.RenameTable(
                name: "ProjectAdmins",
                newName: "ProjectAdmin");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMaintainers_MaintainerId",
                table: "ProjectMaintainer",
                newName: "IX_ProjectMaintainer_MaintainerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectAdmins_AdminId",
                table: "ProjectAdmin",
                newName: "IX_ProjectAdmin_AdminId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IssueId",
                table: "IssueTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Issues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMaintainer",
                table: "ProjectMaintainer",
                columns: new[] { "ProjectId", "MaintainerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectAdmin",
                table: "ProjectAdmin",
                columns: new[] { "ProjectId", "AdminId" });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypes_IssueId",
                table: "IssueTypes",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CompanyId",
                table: "Issues",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTypes_Issues_IssueId",
                table: "IssueTypes",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAdmin_AspNetUsers_AdminId",
                table: "ProjectAdmin",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectAdmin_Projects_ProjectId",
                table: "ProjectAdmin",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMaintainer_AspNetUsers_MaintainerId",
                table: "ProjectMaintainer",
                column: "MaintainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMaintainer_Projects_ProjectId",
                table: "ProjectMaintainer",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

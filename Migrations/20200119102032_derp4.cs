using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class derp4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueStages_StageId",
                table: "Issues");

            migrationBuilder.AlterColumn<Guid>(
                name: "StageId",
                table: "Issues",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Issues",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueStages_StageId",
                table: "Issues",
                column: "StageId",
                principalTable: "IssueStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_IssueStages_StageId",
                table: "Issues");

            migrationBuilder.AlterColumn<Guid>(
                name: "StageId",
                table: "Issues",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Issues",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Organizations_OrganizationId",
                table: "Issues",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_IssueStages_StageId",
                table: "Issues",
                column: "StageId",
                principalTable: "IssueStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

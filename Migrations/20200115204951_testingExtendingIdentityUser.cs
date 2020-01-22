using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class testingExtendingIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_AspNetUsers_UserId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Issues_UserId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Issues");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Projects",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                table: "Projects",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Issues",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Issues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "IssuePosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IssuePostReactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "testu",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId1",
                table: "Projects",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AuthorId",
                table: "Issues",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePosts_AuthorId",
                table: "IssuePosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuePostReactions_UserId",
                table: "IssuePostReactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuePostReactions_AspNetUsers_UserId",
                table: "IssuePostReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuePosts_AspNetUsers_AuthorId",
                table: "IssuePosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_AspNetUsers_AuthorId",
                table: "Issues",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Projects_CompanyId",
                table: "Projects",
                column: "CompanyId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId1",
                table: "Projects",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuePostReactions_AspNetUsers_UserId",
                table: "IssuePostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuePosts_AspNetUsers_AuthorId",
                table: "IssuePosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_AspNetUsers_AuthorId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Projects_CompanyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompanyId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Issues_AuthorId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_IssuePosts_AuthorId",
                table: "IssuePosts");

            migrationBuilder.DropIndex(
                name: "IX_IssuePostReactions_UserId",
                table: "IssuePostReactions");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "IssuePosts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IssuePostReactions");

            migrationBuilder.DropColumn(
                name: "testu",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Projects",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Issues",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Issues",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_UserId",
                table: "Issues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Companies_CompanyId",
                table: "Issues",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_AspNetUsers_UserId",
                table: "Issues",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId",
                table: "Projects",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

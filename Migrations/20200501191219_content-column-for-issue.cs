using Microsoft.EntityFrameworkCore.Migrations;

namespace DragonflyTracker.Migrations
{
    public partial class contentcolumnforissue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Issues",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Issues");
        }
    }
}

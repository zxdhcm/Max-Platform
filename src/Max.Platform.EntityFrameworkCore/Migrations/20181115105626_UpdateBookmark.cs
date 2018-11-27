using Microsoft.EntityFrameworkCore.Migrations;

namespace Max.Platform.Migrations
{
    public partial class UpdateBookmark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookmarkClasss");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BookmarkClasss",
                nullable: false,
                defaultValue: 0);
        }
    }
}

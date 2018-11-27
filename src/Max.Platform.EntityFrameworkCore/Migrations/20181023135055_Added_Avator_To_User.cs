using Microsoft.EntityFrameworkCore.Migrations;

namespace Max.Platform.Migrations
{
    public partial class Added_Avator_To_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avator",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avator",
                table: "AbpUsers");
        }
    }
}

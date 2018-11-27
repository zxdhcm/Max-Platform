using Microsoft.EntityFrameworkCore.Migrations;

namespace Max.Platform.Migrations
{
    public partial class Added_PasswordStrength_To_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordStrength",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordStrength",
                table: "AbpUsers");
        }
    }
}

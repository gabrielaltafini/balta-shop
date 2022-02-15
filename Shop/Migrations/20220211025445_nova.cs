using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Migrations
{
    public partial class nova : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Teste",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Teste",
                table: "Usuario");
        }
    }
}

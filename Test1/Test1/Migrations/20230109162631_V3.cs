using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test1.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminAccess",
                table: "UserDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "resetStatus",
                table: "MasterPassword",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminAccess",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "resetStatus",
                table: "MasterPassword");
        }
    }
}

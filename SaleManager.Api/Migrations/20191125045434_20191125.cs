using Microsoft.EntityFrameworkCore.Migrations;

namespace SaleManager.Api.Migrations
{
    public partial class _20191125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "AspNetUsers");
        }
    }
}

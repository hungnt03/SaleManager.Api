using Microsoft.EntityFrameworkCore.Migrations;

namespace SaleManager.Api.Migrations
{
    public partial class _20191210 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarcodeImg",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Product",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnable",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "BarcodeImg",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnable",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCN.Migrations
{
    public partial class FileMinhChung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileMinhChung",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileMinhChung",
                table: "ThongTinKhac");
        }
    }
}

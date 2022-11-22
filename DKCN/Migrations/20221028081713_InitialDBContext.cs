using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCN.Migrations
{
    public partial class InitialDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongTinKhac",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOTEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GIOITINH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NGAYSINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CMND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NOICAP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAYCAP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MATRUONG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TENTRUONG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGANHHOC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KHOAHOC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HEDAOTAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KHOA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAILSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAYNHAPHOC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NGAYDANGKI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    THOIGIANRATRUONG = table.Column<DateTime>(type: "datetime2", nullable: false),
                    THUOCDIEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    THUOCDOITUONG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LYDO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XACNHAN = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinKhac", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinVayVon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HOTEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GIOITINH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NGAYSINH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CMND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAILSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KHOAHOC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KHOA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HEDAOTAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGAYDANGKI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LYDO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XACNHAN = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinVayVon", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongTinKhac");

            migrationBuilder.DropTable(
                name: "ThongTinVayVon");
        }
    }
}

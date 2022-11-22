using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DKCN.Migrations
{
    public partial class UpdateDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MATRUONG",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "NGAYCAP",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "NGAYNHAPHOC",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "NOICAP",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "TENTRUONG",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "THOIGIANRATRUONG",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "THUOCDIEN",
                table: "ThongTinKhac");

            migrationBuilder.DropColumn(
                name: "THUOCDOITUONG",
                table: "ThongTinKhac");

            migrationBuilder.AddColumn<string>(
                name: "NGANHHOC",
                table: "ThongTinVayVon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NGAYCAP",
                table: "ThongTinVayVon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NGAYNHAPHOC",
                table: "ThongTinVayVon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NOICAP",
                table: "ThongTinVayVon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "THOIGIANRATRUONG",
                table: "ThongTinVayVon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "THUOCDIEN",
                table: "ThongTinVayVon",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "THUOCDOITUONG",
                table: "ThongTinVayVon",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NGANHHOC",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "NGAYCAP",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "NGAYNHAPHOC",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "NOICAP",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "THOIGIANRATRUONG",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "THUOCDIEN",
                table: "ThongTinVayVon");

            migrationBuilder.DropColumn(
                name: "THUOCDOITUONG",
                table: "ThongTinVayVon");

            migrationBuilder.AddColumn<string>(
                name: "MATRUONG",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NGAYCAP",
                table: "ThongTinKhac",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NGAYNHAPHOC",
                table: "ThongTinKhac",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NOICAP",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TENTRUONG",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "THOIGIANRATRUONG",
                table: "ThongTinKhac",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "THUOCDIEN",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "THUOCDOITUONG",
                table: "ThongTinKhac",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

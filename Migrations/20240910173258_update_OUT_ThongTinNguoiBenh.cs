using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class update_OUT_ThongTinNguoiBenh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OUT_DanhGia_OUT_ThongTinNguoiBenh_ThongTinNguoiBenhIdOUT_Th~",
                table: "OUT_DanhGia");

            migrationBuilder.AddColumn<int>(
                name: "KhoangCach",
                table: "OUT_ThongTinNguoiBenh",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idAdmin",
                table: "OUT_DanhGia",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedDate",
                table: "Admins",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_OUT_DanhGia_OUT_ThongTinNguoiBenh_ThongTinNguoiBenhIdOUT_Th~",
                table: "OUT_DanhGia",
                column: "ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh",
                principalTable: "OUT_ThongTinNguoiBenh",
                principalColumn: "IdOUT_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OUT_DanhGia_OUT_ThongTinNguoiBenh_ThongTinNguoiBenhIdOUT_Th~",
                table: "OUT_DanhGia");

            migrationBuilder.DropColumn(
                name: "KhoangCach",
                table: "OUT_ThongTinNguoiBenh");

            migrationBuilder.AlterColumn<int>(
                name: "idAdmin",
                table: "OUT_DanhGia",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Admins",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_OUT_DanhGia_OUT_ThongTinNguoiBenh_ThongTinNguoiBenhIdOUT_Th~",
                table: "OUT_DanhGia",
                column: "ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh",
                principalTable: "OUT_ThongTinNguoiBenh",
                principalColumn: "IdOUT_ThongTinNguoiBenh",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_OUT_MauKhaoSat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayTao",
                table: "OUT_ThongTinYKienKhac",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDienPhieu",
                table: "OUT_ThongTinChung",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");

            migrationBuilder.AddColumn<bool>(
                name: "HienThi",
                table: "OUT_MauKhaoSat",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Xoa",
                table: "OUT_MauKhaoSat",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayTao",
                table: "IN_ThongTinYKienKhac",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDienPhieu",
                table: "IN_ThongTinChung",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HienThi",
                table: "OUT_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "Xoa",
                table: "OUT_MauKhaoSat");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayTao",
                table: "OUT_ThongTinYKienKhac",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDienPhieu",
                table: "OUT_ThongTinChung",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayTao",
                table: "IN_ThongTinYKienKhac",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDienPhieu",
                table: "IN_ThongTinChung",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);
        }
    }
}

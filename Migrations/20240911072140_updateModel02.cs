using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateModel02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "OUT_MauKhaoSat",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrangThai",
                table: "IN_MauKhaoSat",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDanhGia",
                table: "IN_DanhGia",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "OUT_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "IN_MauKhaoSat");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NgayDanhGia",
                table: "IN_DanhGia",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);
        }
    }
}

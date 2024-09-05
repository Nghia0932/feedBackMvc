using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class addColumnIN_INMauKhaoSatMauKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDau",
                table: "IN_MauKhaoSat",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThuc",
                table: "IN_MauKhaoSat",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoluongKhaoSat",
                table: "IN_MauKhaoSat",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "IN_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "NgayKetThuc",
                table: "IN_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "SoluongKhaoSat",
                table: "IN_MauKhaoSat");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_MauKHaoSat02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "MucDanhGia",
                table: "OUT_MauKhaoSat",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "MucDanhGia",
                table: "IN_MauKhaoSat",
                type: "integer[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MucDanhGia",
                table: "OUT_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "MucDanhGia",
                table: "IN_MauKhaoSat");
        }
    }
}

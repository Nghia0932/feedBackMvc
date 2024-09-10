using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OUT_DanhGia_IdOUT_MauKhaoSat_IdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_DanhGia_IdOUT_MauKhaoSat_IdOUT_ThongTinNguoiBenh_NgayDa~",
                table: "OUT_DanhGia",
                columns: new[] { "IdOUT_MauKhaoSat", "IdOUT_ThongTinNguoiBenh", "NgayDanhGia" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OUT_DanhGia_IdOUT_MauKhaoSat_IdOUT_ThongTinNguoiBenh_NgayDa~",
                table: "OUT_DanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_DanhGia_IdOUT_MauKhaoSat_IdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia",
                columns: new[] { "IdOUT_MauKhaoSat", "IdOUT_ThongTinNguoiBenh" },
                unique: true);
        }
    }
}

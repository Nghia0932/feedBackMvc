using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateModel_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_ThongTinNguoiBenhIdIN_Thong~",
                table: "IN_DanhGia");

            migrationBuilder.DropIndex(
                name: "IX_IN_DanhGia_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia");

            migrationBuilder.DropColumn(
                name: "ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia");

            migrationBuilder.AddColumn<int>(
                name: "IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                column: "IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh");

            migrationBuilder.AddForeignKey(
                name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_IN_ThongTinNguoiBenhIdIN_Th~",
                table: "IN_DanhGia",
                column: "IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                principalTable: "IN_ThongTinNguoiBenh",
                principalColumn: "IdIN_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_IN_ThongTinNguoiBenhIdIN_Th~",
                table: "IN_DanhGia");

            migrationBuilder.DropIndex(
                name: "IX_IN_DanhGia_IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia");

            migrationBuilder.DropColumn(
                name: "IN_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia");

            migrationBuilder.AddColumn<int>(
                name: "ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                column: "ThongTinNguoiBenhIdIN_ThongTinNguoiBenh");

            migrationBuilder.AddForeignKey(
                name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_ThongTinNguoiBenhIdIN_Thong~",
                table: "IN_DanhGia",
                column: "ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                principalTable: "IN_ThongTinNguoiBenh",
                principalColumn: "IdIN_ThongTinNguoiBenh",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class ORTHER_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORTHER_MauKhaoSat",
                columns: table => new
                {
                    IdORTHER_MauKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayTao = table.Column<DateOnly>(type: "DATE", nullable: false),
                    TenMauKhaoSat = table.Column<string>(type: "text", nullable: true),
                    NhomCauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    CauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    idAdmin = table.Column<int>(type: "integer", nullable: false),
                    NgayBatDau = table.Column<DateOnly>(type: "DATE", nullable: true),
                    NgayKetThuc = table.Column<DateOnly>(type: "DATE", nullable: true),
                    TrangThai = table.Column<bool>(type: "boolean", nullable: true),
                    SoluongKhaoSat = table.Column<int>(type: "integer", nullable: true),
                    HienThi = table.Column<bool>(type: "boolean", nullable: true),
                    Xoa = table.Column<bool>(type: "boolean", nullable: true),
                    MucQuanTrong = table.Column<double[]>(type: "double precision[]", nullable: true),
                    MucDanhGia = table.Column<int[]>(type: "integer[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_MauKhaoSat", x => x.IdORTHER_MauKhaoSat);
                    table.ForeignKey(
                        name: "FK_ORTHER_MauKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORTHER_NhomCauHoiKhaoSat",
                columns: table => new
                {
                    IdORTHER_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDe = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_NhomCauHoiKhaoSat", x => x.IdORTHER_NhomCauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_ORTHER_NhomCauHoiKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORTHER_ThongTinNguoiDanhGia",
                columns: table => new
                {
                    IdORTHER_ThongTinNguoiDanhGia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GioiTinh = table.Column<string>(type: "text", nullable: true),
                    Tuoi = table.Column<int>(type: "integer", nullable: true),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_ThongTinNguoiDanhGia", x => x.IdORTHER_ThongTinNguoiDanhGia);
                });

            migrationBuilder.CreateTable(
                name: "ORTHER_CauHoiKhaoSat",
                columns: table => new
                {
                    IdORTHER_CauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDeCauHoi = table.Column<string>(type: "text", nullable: false),
                    CauHoi = table.Column<string>(type: "text", nullable: false),
                    IdORTHER_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_CauHoiKhaoSat", x => x.IdORTHER_CauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_ORTHER_CauHoiKhaoSat_ORTHER_NhomCauHoiKhaoSat_IdORTHER_Nhom~",
                        column: x => x.IdORTHER_NhomCauHoiKhaoSat,
                        principalTable: "ORTHER_NhomCauHoiKhaoSat",
                        principalColumn: "IdORTHER_NhomCauHoiKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORTHER_DanhGia",
                columns: table => new
                {
                    IdORTHER_DanhGia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayDanhGia = table.Column<DateOnly>(type: "DATE", nullable: true),
                    DanhGia = table.Column<int[]>(type: "int[]", nullable: false),
                    DanhGiaTong = table.Column<double[]>(type: "double precision[]", nullable: true),
                    IdORTHER_MauKhaoSat = table.Column<int>(type: "integer", nullable: false),
                    IdORTHER_ThongTinNguoiDanhGia = table.Column<int>(type: "integer", nullable: false),
                    ORTHER_ThongTinNguoiDanhGiaIdORTHER_ThongTinNguoiDanhGia = table.Column<int>(type: "integer", nullable: true),
                    idAdmin = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_DanhGia", x => x.IdORTHER_DanhGia);
                    table.ForeignKey(
                        name: "FK_ORTHER_DanhGia_ORTHER_MauKhaoSat_IdORTHER_MauKhaoSat",
                        column: x => x.IdORTHER_MauKhaoSat,
                        principalTable: "ORTHER_MauKhaoSat",
                        principalColumn: "IdORTHER_MauKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORTHER_DanhGia_ORTHER_ThongTinNguoiDanhGia_ORTHER_ThongTinN~",
                        column: x => x.ORTHER_ThongTinNguoiDanhGiaIdORTHER_ThongTinNguoiDanhGia,
                        principalTable: "ORTHER_ThongTinNguoiDanhGia",
                        principalColumn: "IdORTHER_ThongTinNguoiDanhGia");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_CauHoiKhaoSat_IdORTHER_NhomCauHoiKhaoSat",
                table: "ORTHER_CauHoiKhaoSat",
                column: "IdORTHER_NhomCauHoiKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_CauHoiKhaoSat_TieuDeCauHoi",
                table: "ORTHER_CauHoiKhaoSat",
                column: "TieuDeCauHoi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_DanhGia_IdORTHER_MauKhaoSat_IdORTHER_ThongTinNguoiDa~",
                table: "ORTHER_DanhGia",
                columns: new[] { "IdORTHER_MauKhaoSat", "IdORTHER_ThongTinNguoiDanhGia", "NgayDanhGia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_DanhGia_ORTHER_ThongTinNguoiDanhGiaIdORTHER_ThongTin~",
                table: "ORTHER_DanhGia",
                column: "ORTHER_ThongTinNguoiDanhGiaIdORTHER_ThongTinNguoiDanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_MauKhaoSat_idAdmin",
                table: "ORTHER_MauKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_NhomCauHoiKhaoSat_idAdmin",
                table: "ORTHER_NhomCauHoiKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_NhomCauHoiKhaoSat_TieuDe",
                table: "ORTHER_NhomCauHoiKhaoSat",
                column: "TieuDe",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORTHER_CauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "ORTHER_DanhGia");

            migrationBuilder.DropTable(
                name: "ORTHER_NhomCauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "ORTHER_MauKhaoSat");

            migrationBuilder.DropTable(
                name: "ORTHER_ThongTinNguoiDanhGia");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ten = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MatKhau = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.idAdmin);
                });

            migrationBuilder.CreateTable(
                name: "IN_ThongTinNguoiBenh",
                columns: table => new
                {
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GioiTinh = table.Column<string>(type: "text", nullable: true),
                    Tuoi = table.Column<int>(type: "integer", nullable: true),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false),
                    SoNgayNamVien = table.Column<int>(type: "integer", nullable: true),
                    CoSuDungBHYT = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_ThongTinNguoiBenh", x => x.IdIN_ThongTinNguoiBenh);
                });

            migrationBuilder.CreateTable(
                name: "OUT_ThongTinNguoiBenh",
                columns: table => new
                {
                    IdOUT_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GioiTinh = table.Column<string>(type: "text", nullable: true),
                    Tuoi = table.Column<int>(type: "integer", nullable: true),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false),
                    SoNgayNamVien = table.Column<int>(type: "integer", nullable: true),
                    CoSuDungBHYT = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_ThongTinNguoiBenh", x => x.IdOUT_ThongTinNguoiBenh);
                });

            migrationBuilder.CreateTable(
                name: "IN_MauKhaoSat",
                columns: table => new
                {
                    IdIN_MauKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenMauKhaoSat = table.Column<string>(type: "text", nullable: true),
                    NhomCauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    CauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    idAdmin = table.Column<int>(type: "integer", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SoluongKhaoSat = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_MauKhaoSat", x => x.IdIN_MauKhaoSat);
                    table.ForeignKey(
                        name: "FK_IN_MauKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IN_NhomCauHoiKhaoSat",
                columns: table => new
                {
                    IdIN_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDe = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_NhomCauHoiKhaoSat", x => x.IdIN_NhomCauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_IN_NhomCauHoiKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_MauKhaoSat",
                columns: table => new
                {
                    IdOUT_MauKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenMauKhaoSat = table.Column<string>(type: "text", nullable: true),
                    NhomCauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    CauHoiKhaoSat = table.Column<string[]>(type: "text[]", nullable: true),
                    idAdmin = table.Column<int>(type: "integer", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SoluongKhaoSat = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_MauKhaoSat", x => x.IdOUT_MauKhaoSat);
                    table.ForeignKey(
                        name: "FK_OUT_MauKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_NhomCauHoiKhaoSat",
                columns: table => new
                {
                    IdOUT_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDe = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_NhomCauHoiKhaoSat", x => x.IdOUT_NhomCauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_OUT_NhomCauHoiKhaoSat_Admins_idAdmin",
                        column: x => x.idAdmin,
                        principalTable: "Admins",
                        principalColumn: "idAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IN_ThongTinChung",
                columns: table => new
                {
                    IdIN_ThongTinChung = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenBenhVien = table.Column<string>(type: "text", nullable: true),
                    NgayDienPhieu = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NguoiTraLoi = table.Column<string>(type: "text", nullable: true),
                    TenKhoa = table.Column<string>(type: "text", nullable: true),
                    MaKhoa = table.Column<string>(type: "text", nullable: true),
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_ThongTinChung", x => x.IdIN_ThongTinChung);
                    table.ForeignKey(
                        name: "FK_IN_ThongTinChung_IN_ThongTinNguoiBenh_IdIN_ThongTinNguoiBenh",
                        column: x => x.IdIN_ThongTinNguoiBenh,
                        principalTable: "IN_ThongTinNguoiBenh",
                        principalColumn: "IdIN_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IN_ThongTinYKienKhac",
                columns: table => new
                {
                    IdIN_ThongTinYKienKhac = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhanTramMongDoi = table.Column<int>(type: "integer", nullable: true),
                    QuayLaiVaGioiThieu = table.Column<string>(type: "text", nullable: true),
                    YKienKhac = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_ThongTinYKienKhac", x => x.IdIN_ThongTinYKienKhac);
                    table.ForeignKey(
                        name: "FK_IN_ThongTinYKienKhac_IN_ThongTinNguoiBenh_IdIN_ThongTinNguo~",
                        column: x => x.IdIN_ThongTinNguoiBenh,
                        principalTable: "IN_ThongTinNguoiBenh",
                        principalColumn: "IdIN_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_ThongTinChung",
                columns: table => new
                {
                    IdOUT_ThongTinChung = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenBenhVien = table.Column<string>(type: "text", nullable: true),
                    NgayDienPhieu = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NguoiTraLoi = table.Column<string>(type: "text", nullable: true),
                    TenKhoa = table.Column<string>(type: "text", nullable: true),
                    MaKhoa = table.Column<string>(type: "text", nullable: true),
                    IdOUT_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_ThongTinChung", x => x.IdOUT_ThongTinChung);
                    table.ForeignKey(
                        name: "FK_OUT_ThongTinChung_OUT_ThongTinNguoiBenh_IdOUT_ThongTinNguoi~",
                        column: x => x.IdOUT_ThongTinNguoiBenh,
                        principalTable: "OUT_ThongTinNguoiBenh",
                        principalColumn: "IdOUT_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_ThongTinYKienKhac",
                columns: table => new
                {
                    IdOUT_ThongTinYKienKhac = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhanTramMongDoi = table.Column<int>(type: "integer", nullable: true),
                    QuayLaiVaGioiThieu = table.Column<string>(type: "text", nullable: true),
                    YKienKhac = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdOUT_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_ThongTinYKienKhac", x => x.IdOUT_ThongTinYKienKhac);
                    table.ForeignKey(
                        name: "FK_OUT_ThongTinYKienKhac_OUT_ThongTinNguoiBenh_IdOUT_ThongTinN~",
                        column: x => x.IdOUT_ThongTinNguoiBenh,
                        principalTable: "OUT_ThongTinNguoiBenh",
                        principalColumn: "IdOUT_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IN_DanhGia",
                columns: table => new
                {
                    IdIN_DanhGia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayDanhGia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DanhGia = table.Column<int[]>(type: "int[]", nullable: false),
                    IdIN_MauKhaoSat = table.Column<int>(type: "integer", nullable: false),
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false),
                    ThongTinNguoiBenhIdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false),
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_DanhGia", x => x.IdIN_DanhGia);
                    table.ForeignKey(
                        name: "FK_IN_DanhGia_IN_MauKhaoSat_IdIN_MauKhaoSat",
                        column: x => x.IdIN_MauKhaoSat,
                        principalTable: "IN_MauKhaoSat",
                        principalColumn: "IdIN_MauKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_ThongTinNguoiBenhIdIN_Thong~",
                        column: x => x.ThongTinNguoiBenhIdIN_ThongTinNguoiBenh,
                        principalTable: "IN_ThongTinNguoiBenh",
                        principalColumn: "IdIN_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IN_CauHoiKhaoSat",
                columns: table => new
                {
                    IdIN_CauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDeCauHoi = table.Column<string>(type: "text", nullable: false),
                    CauHoi = table.Column<string>(type: "text", nullable: false),
                    IdIN_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_CauHoiKhaoSat", x => x.IdIN_CauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_IN_CauHoiKhaoSat_IN_NhomCauHoiKhaoSat_IdIN_NhomCauHoiKhaoSat",
                        column: x => x.IdIN_NhomCauHoiKhaoSat,
                        principalTable: "IN_NhomCauHoiKhaoSat",
                        principalColumn: "IdIN_NhomCauHoiKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_DanhGia",
                columns: table => new
                {
                    IdOUT_DanhGia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayDanhGia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DanhGia = table.Column<int[]>(type: "integer[]", nullable: false),
                    IdOUT_MauKhaoSat = table.Column<int>(type: "integer", nullable: false),
                    IdOUT_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false),
                    ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false),
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_DanhGia", x => x.IdOUT_DanhGia);
                    table.ForeignKey(
                        name: "FK_OUT_DanhGia_OUT_MauKhaoSat_IdOUT_MauKhaoSat",
                        column: x => x.IdOUT_MauKhaoSat,
                        principalTable: "OUT_MauKhaoSat",
                        principalColumn: "IdOUT_MauKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUT_DanhGia_OUT_ThongTinNguoiBenh_ThongTinNguoiBenhIdOUT_Th~",
                        column: x => x.ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh,
                        principalTable: "OUT_ThongTinNguoiBenh",
                        principalColumn: "IdOUT_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUT_CauHoiKhaoSat",
                columns: table => new
                {
                    IdOUT_CauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDeCauHoi = table.Column<string>(type: "text", nullable: false),
                    CauHoi = table.Column<string>(type: "text", nullable: false),
                    IdOUT_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUT_CauHoiKhaoSat", x => x.IdOUT_CauHoiKhaoSat);
                    table.ForeignKey(
                        name: "FK_OUT_CauHoiKhaoSat_OUT_NhomCauHoiKhaoSat_IdOUT_NhomCauHoiKha~",
                        column: x => x.IdOUT_NhomCauHoiKhaoSat,
                        principalTable: "OUT_NhomCauHoiKhaoSat",
                        principalColumn: "IdOUT_NhomCauHoiKhaoSat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IN_CauHoiKhaoSat_IdIN_NhomCauHoiKhaoSat",
                table: "IN_CauHoiKhaoSat",
                column: "IdIN_NhomCauHoiKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_IN_CauHoiKhaoSat_TieuDeCauHoi",
                table: "IN_CauHoiKhaoSat",
                column: "TieuDeCauHoi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_IdIN_MauKhaoSat_IdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                columns: new[] { "IdIN_MauKhaoSat", "IdIN_ThongTinNguoiBenh" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_ThongTinNguoiBenhIdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                column: "ThongTinNguoiBenhIdIN_ThongTinNguoiBenh");

            migrationBuilder.CreateIndex(
                name: "IX_IN_MauKhaoSat_idAdmin",
                table: "IN_MauKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_IN_NhomCauHoiKhaoSat_idAdmin",
                table: "IN_NhomCauHoiKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_IN_NhomCauHoiKhaoSat_TieuDe",
                table: "IN_NhomCauHoiKhaoSat",
                column: "TieuDe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IN_ThongTinChung_IdIN_ThongTinNguoiBenh",
                table: "IN_ThongTinChung",
                column: "IdIN_ThongTinNguoiBenh");

            migrationBuilder.CreateIndex(
                name: "IX_IN_ThongTinYKienKhac_IdIN_ThongTinNguoiBenh",
                table: "IN_ThongTinYKienKhac",
                column: "IdIN_ThongTinNguoiBenh");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_CauHoiKhaoSat_IdOUT_NhomCauHoiKhaoSat",
                table: "OUT_CauHoiKhaoSat",
                column: "IdOUT_NhomCauHoiKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_CauHoiKhaoSat_TieuDeCauHoi",
                table: "OUT_CauHoiKhaoSat",
                column: "TieuDeCauHoi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUT_DanhGia_IdOUT_MauKhaoSat_IdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia",
                columns: new[] { "IdOUT_MauKhaoSat", "IdOUT_ThongTinNguoiBenh" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUT_DanhGia_ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh",
                table: "OUT_DanhGia",
                column: "ThongTinNguoiBenhIdOUT_ThongTinNguoiBenh");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_MauKhaoSat_idAdmin",
                table: "OUT_MauKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_NhomCauHoiKhaoSat_idAdmin",
                table: "OUT_NhomCauHoiKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_NhomCauHoiKhaoSat_TieuDe",
                table: "OUT_NhomCauHoiKhaoSat",
                column: "TieuDe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUT_ThongTinChung_IdOUT_ThongTinNguoiBenh",
                table: "OUT_ThongTinChung",
                column: "IdOUT_ThongTinNguoiBenh");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_ThongTinYKienKhac_IdOUT_ThongTinNguoiBenh",
                table: "OUT_ThongTinYKienKhac",
                column: "IdOUT_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_CauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "IN_DanhGia");

            migrationBuilder.DropTable(
                name: "IN_ThongTinChung");

            migrationBuilder.DropTable(
                name: "IN_ThongTinYKienKhac");

            migrationBuilder.DropTable(
                name: "OUT_CauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "OUT_DanhGia");

            migrationBuilder.DropTable(
                name: "OUT_ThongTinChung");

            migrationBuilder.DropTable(
                name: "OUT_ThongTinYKienKhac");

            migrationBuilder.DropTable(
                name: "IN_NhomCauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "IN_MauKhaoSat");

            migrationBuilder.DropTable(
                name: "IN_ThongTinNguoiBenh");

            migrationBuilder.DropTable(
                name: "OUT_NhomCauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "OUT_MauKhaoSat");

            migrationBuilder.DropTable(
                name: "OUT_ThongTinNguoiBenh");

            migrationBuilder.DropTable(
                name: "Admins");
        }
    }
}

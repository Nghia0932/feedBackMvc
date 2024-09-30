using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class ORTHER_ThongTinYKienKhacModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORTHER_ThongTinYKienKhac",
                columns: table => new
                {
                    IdORTHER_ThongTinYKienKhac = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhanTramMongDoi = table.Column<int>(type: "integer", nullable: true),
                    QuayLaiVaGioiThieu = table.Column<string>(type: "text", nullable: true),
                    YKienKhac = table.Column<string>(type: "text", nullable: true),
                    NgayTao = table.Column<DateOnly>(type: "DATE", nullable: true),
                    IdORTHER_ThongTinNguoiDanhGia = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORTHER_ThongTinYKienKhac", x => x.IdORTHER_ThongTinYKienKhac);
                    table.ForeignKey(
                        name: "FK_ORTHER_ThongTinYKienKhac_ORTHER_ThongTinNguoiDanhGia_IdORTH~",
                        column: x => x.IdORTHER_ThongTinNguoiDanhGia,
                        principalTable: "ORTHER_ThongTinNguoiDanhGia",
                        principalColumn: "IdORTHER_ThongTinNguoiDanhGia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORTHER_ThongTinYKienKhac_IdORTHER_ThongTinNguoiDanhGia",
                table: "ORTHER_ThongTinYKienKhac",
                column: "IdORTHER_ThongTinNguoiDanhGia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORTHER_ThongTinYKienKhac");
        }
    }
}

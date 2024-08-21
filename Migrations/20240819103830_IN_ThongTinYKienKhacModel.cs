using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_ThongTinYKienKhacModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IN_ThongTinYKienKhac",
                columns: table => new
                {
                    IdIN_ThongTinYKienKhac = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhanTramMongDoi = table.Column<int>(type: "integer", nullable: true),
                    QuayLaiVaGioithieu = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_IN_ThongTinYKienKhac_IdIN_ThongTinNguoiBenh",
                table: "IN_ThongTinYKienKhac",
                column: "IdIN_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_ThongTinYKienKhac");
        }
    }
}

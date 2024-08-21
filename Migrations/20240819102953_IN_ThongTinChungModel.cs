using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_ThongTinChungModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_IN_ThongTinChung_IdIN_ThongTinNguoiBenh",
                table: "IN_ThongTinChung",
                column: "IdIN_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_ThongTinChung");
        }
    }
}

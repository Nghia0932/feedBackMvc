using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_DanhGiaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IN_DanhGia",
                columns: table => new
                {
                    IdIN_DanhGia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NgayDanhGia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DanhGia = table.Column<int[]>(type: "int[]", nullable: false),
                    IdIN_MauKhaoSat = table.Column<int>(type: "integer", nullable: false),
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_IN_DanhGia_IN_ThongTinNguoiBenh_IdIN_ThongTinNguoiBenh",
                        column: x => x.IdIN_ThongTinNguoiBenh,
                        principalTable: "IN_ThongTinNguoiBenh",
                        principalColumn: "IdIN_ThongTinNguoiBenh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_IdIN_MauKhaoSat",
                table: "IN_DanhGia",
                column: "IdIN_MauKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_IN_DanhGia_IdIN_ThongTinNguoiBenh",
                table: "IN_DanhGia",
                column: "IdIN_ThongTinNguoiBenh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_DanhGia");
        }
    }
}

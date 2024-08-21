using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_ThongTinNguoiBenhModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IN_ThongTinNguoiBenh",
                columns: table => new
                {
                    IdIN_ThongTinNguoiBenh = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GioiTinh = table.Column<string>(type: "text", nullable: true),
                    Tuoi = table.Column<int>(type: "integer", nullable: true),
                    SoDienThoai = table.Column<string>(type: "text", nullable: false),
                    SoNgayNamVien = table.Column<string>(type: "text", nullable: true),
                    CoSuDungBHYT = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IN_ThongTinNguoiBenh", x => x.IdIN_ThongTinNguoiBenh);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_ThongTinNguoiBenh");
        }
    }
}

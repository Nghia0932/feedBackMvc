using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_CauHoiKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IN_CauHoiKhaoSat",
                columns: table => new
                {
                    IdIN_CauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDeCauHoi = table.Column<char>(type: "character(1)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_IN_CauHoiKhaoSat_IdIN_NhomCauHoiKhaoSat",
                table: "IN_CauHoiKhaoSat",
                column: "IdIN_NhomCauHoiKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_IN_CauHoiKhaoSat_TieuDeCauHoi",
                table: "IN_CauHoiKhaoSat",
                column: "TieuDeCauHoi",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_CauHoiKhaoSat");
        }
    }
}

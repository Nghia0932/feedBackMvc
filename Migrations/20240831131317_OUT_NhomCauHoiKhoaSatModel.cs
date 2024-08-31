using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class OUT_NhomCauHoiKhoaSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_OUT_CauHoiKhaoSat_IdOUT_NhomCauHoiKhaoSat",
                table: "OUT_CauHoiKhaoSat",
                column: "IdOUT_NhomCauHoiKhaoSat");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_CauHoiKhaoSat_TieuDeCauHoi",
                table: "OUT_CauHoiKhaoSat",
                column: "TieuDeCauHoi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUT_NhomCauHoiKhaoSat_idAdmin",
                table: "OUT_NhomCauHoiKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_OUT_NhomCauHoiKhaoSat_TieuDe",
                table: "OUT_NhomCauHoiKhaoSat",
                column: "TieuDe",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUT_CauHoiKhaoSat");

            migrationBuilder.DropTable(
                name: "OUT_NhomCauHoiKhaoSat");
        }
    }
}

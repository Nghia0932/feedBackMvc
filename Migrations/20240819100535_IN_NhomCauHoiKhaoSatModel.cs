using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_NhomCauHoiKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IN_NhomCauHoiKhaoSat",
                columns: table => new
                {
                    IdIN_NhomCauHoiKhaoSat = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TieuDe = table.Column<char>(type: "character(1)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_IN_NhomCauHoiKhaoSat_idAdmin",
                table: "IN_NhomCauHoiKhaoSat",
                column: "idAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_IN_NhomCauHoiKhaoSat_TieuDe",
                table: "IN_NhomCauHoiKhaoSat",
                column: "TieuDe",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_NhomCauHoiKhaoSat");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class OUT_MauKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_OUT_MauKhaoSat_idAdmin",
                table: "OUT_MauKhaoSat",
                column: "idAdmin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUT_MauKhaoSat");
        }
    }
}

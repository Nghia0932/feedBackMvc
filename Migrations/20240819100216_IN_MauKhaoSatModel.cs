using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class IN_MauKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    idAdmin = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_IN_MauKhaoSat_idAdmin",
                table: "IN_MauKhaoSat",
                column: "idAdmin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IN_MauKhaoSat");
        }
    }
}

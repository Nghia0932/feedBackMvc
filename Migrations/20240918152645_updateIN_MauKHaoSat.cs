using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_MauKHaoSat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double[]>(
                name: "MucQuanTrong",
                table: "OUT_MauKhaoSat",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<double[]>(
                name: "MucQuanTrong",
                table: "IN_MauKhaoSat",
                type: "double precision[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MucQuanTrong",
                table: "OUT_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "MucQuanTrong",
                table: "IN_MauKhaoSat");
        }
    }
}

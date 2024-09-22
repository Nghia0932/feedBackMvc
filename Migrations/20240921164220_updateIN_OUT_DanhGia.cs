using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_OUT_DanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double[]>(
                name: "DanhGiaTong",
                table: "OUT_DanhGia",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<double[]>(
                name: "DanhGiaTong",
                table: "IN_DanhGia",
                type: "double precision[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DanhGiaTong",
                table: "OUT_DanhGia");

            migrationBuilder.DropColumn(
                name: "DanhGiaTong",
                table: "IN_DanhGia");
        }
    }
}

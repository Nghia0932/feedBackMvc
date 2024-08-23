using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTieuDeCauHoiType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TieuDeCauHoi",
                table: "IN_CauHoiKhaoSat",
                type: "text",
                nullable: false,
                oldClrType: typeof(char),
                oldType: "character(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "TieuDeCauHoi",
                table: "IN_CauHoiKhaoSat",
                type: "character(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

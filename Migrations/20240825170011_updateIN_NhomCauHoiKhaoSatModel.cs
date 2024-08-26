using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_NhomCauHoiKhaoSatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TieuDe",
                table: "IN_NhomCauHoiKhaoSat",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(char),
                oldType: "character(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "TieuDe",
                table: "IN_NhomCauHoiKhaoSat",
                type: "character(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);
        }
    }
}

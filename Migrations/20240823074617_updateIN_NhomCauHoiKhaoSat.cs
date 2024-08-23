using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN_NhomCauHoiKhaoSat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<int>(
                name: "SoNgayNamVien",
                table: "IN_ThongTinNguoiBenh",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
    
            migrationBuilder.AlterColumn<string>(
                name: "SoNgayNamVien",
                table: "IN_ThongTinNguoiBenh",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace feedBackMvc.Migrations
{
    /// <inheritdoc />
    public partial class updateIN__MauKhaoSat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HienThi",
                table: "IN_MauKhaoSat",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Xoa",
                table: "IN_MauKhaoSat",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HienThi",
                table: "IN_MauKhaoSat");

            migrationBuilder.DropColumn(
                name: "Xoa",
                table: "IN_MauKhaoSat");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPersonalizadoFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalizado",
                table: "Pasteles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 101,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 102,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 103,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 104,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 105,
                column: "IsPersonalizado",
                value: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 106,
                column: "IsPersonalizado",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPersonalizado",
                table: "Pasteles");
        }
    }
}

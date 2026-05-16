using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEstatusMerma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estatus",
                table: "Mermas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estatus",
                value: "Pendiente");

            migrationBuilder.UpdateData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Estatus",
                value: "Pendiente");

            migrationBuilder.UpdateData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 103,
                column: "Estatus",
                value: "Pendiente");

            migrationBuilder.UpdateData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 104,
                column: "Estatus",
                value: "Pendiente");

            migrationBuilder.UpdateData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 105,
                column: "Estatus",
                value: "Pendiente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Mermas");
        }
    }
}

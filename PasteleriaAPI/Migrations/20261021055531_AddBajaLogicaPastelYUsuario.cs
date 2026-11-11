using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBajaLogicaPastelYUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Pasteles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 101,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 102,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 103,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 104,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 105,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 106,
                column: "Activo",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "AspNetUsers");
        }
    }
}

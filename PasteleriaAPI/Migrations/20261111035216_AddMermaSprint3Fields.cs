using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMermaSprint3Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mermas_Pasteles_PastelId",
                table: "Mermas");

            migrationBuilder.AlterColumn<int>(
                name: "PastelId",
                table: "Mermas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "Cantidad",
                table: "Mermas",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Causa",
                table: "Mermas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CostoEstimado",
                table: "Mermas",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ElementoAfectado",
                table: "Mermas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoElemento",
                table: "Mermas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Mermas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Mermas_Pasteles_PastelId",
                table: "Mermas",
                column: "PastelId",
                principalTable: "Pasteles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mermas_Pasteles_PastelId",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "Causa",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "CostoEstimado",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "ElementoAfectado",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "TipoElemento",
                table: "Mermas");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Mermas");

            migrationBuilder.AlterColumn<int>(
                name: "PastelId",
                table: "Mermas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mermas_Pasteles_PastelId",
                table: "Mermas",
                column: "PastelId",
                principalTable: "Pasteles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPedidoNotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Pedidos");
        }
    }
}

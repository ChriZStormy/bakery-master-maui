using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Sprint2_PedidoTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntregado",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaListo",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPreparacion",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRecibido",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaEntregado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaListo",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaPreparacion",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaRecibido",
                table: "Pedidos");
        }
    }
}

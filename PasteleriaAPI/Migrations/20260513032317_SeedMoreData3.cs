using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CatBizcochos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 5, "Mármol" },
                    { 6, "Limón" },
                    { 7, "Naranja" },
                    { 8, "Esponja" }
                });

            migrationBuilder.InsertData(
                table: "CatCategorias",
                columns: new[] { "Id", "categoria" },
                values: new object[,]
                {
                    { 6, "Quinceañera" },
                    { 7, "Temático" },
                    { 8, "Vegano" },
                    { 9, "Aniversario" }
                });

            migrationBuilder.InsertData(
                table: "CatGlaseados",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 5, "Merengue Italiano" },
                    { 6, "Espejo de Chocolate" },
                    { 7, "Chantilly" }
                });

            migrationBuilder.InsertData(
                table: "CatRellenos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 5, "Mermelada de Frambuesa" },
                    { 6, "Crema de Limón" },
                    { 7, "Durazno" },
                    { 8, "Frutos Rojos" }
                });

            migrationBuilder.InsertData(
                table: "Mermas",
                columns: new[] { "Id", "Descripcion", "Fecha", "PastelId", "UsuarioId" },
                values: new object[,]
                {
                    { 104, "Flores de fondant derretidas por el calor", new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 105, "La leche se derramó en la caja", new DateTime(2023, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FotoUrl",
                value: "https://images.unsplash.com/photo-1578985545062-69928b1d9587");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FotoUrl",
                value: "https://images.unsplash.com/photo-1557925923-33b251dc32b0");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FotoUrl",
                value: "https://images.unsplash.com/photo-1535141192574-5d4897c12636");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                column: "FotoUrl",
                value: "https://images.unsplash.com/photo-1535254973040-607b474cb50d");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                column: "FotoUrl",
                value: "https://images.unsplash.com/photo-1533134242443-d4fd215305ad");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId" },
                values: new object[] { 8, "https://images.unsplash.com/photo-1464349095431-e9a21285b5f3", 7 });

            migrationBuilder.InsertData(
                table: "Pasteles",
                columns: new[] { "Id", "BizcochoId", "CategoriaId", "FotoUrl", "GlaseadoId", "Nombre", "RellenoId", "Tamanio" },
                values: new object[,]
                {
                    { 103, 2, 1, "https://images.unsplash.com/photo-1550617931-e17a7b70dce2", 3, "Moka Intenso", 4, "Mediano" },
                    { 104, 1, 2, "https://images.unsplash.com/photo-1616541823729-00fe0aacd32c", 1, "Unicornio Mágico", 1, "Grande" },
                    { 106, 2, 1, "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62", 3, "Chocolate Extremo", 4, "Extra Grande" }
                });

            migrationBuilder.InsertData(
                table: "Pedidos",
                columns: new[] { "Id", "Cantidad", "Estatus", "Fecha", "PastelId", "UsuarioId" },
                values: new object[,]
                {
                    { 104, 1, "Entregado", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 106, 1, "Cancelado", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 110, 2, "Entregado", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 }
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "Admin Master");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "Cliente Juan");

            migrationBuilder.InsertData(
                table: "Mermas",
                columns: new[] { "Id", "Descripcion", "Fecha", "PastelId", "UsuarioId" },
                values: new object[] { 103, "El cliente reportó que el chocolate estaba muy amargo", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 106, 2 });

            migrationBuilder.InsertData(
                table: "Pasteles",
                columns: new[] { "Id", "BizcochoId", "CategoriaId", "FotoUrl", "GlaseadoId", "Nombre", "RellenoId", "Tamanio" },
                values: new object[,]
                {
                    { 101, 4, 8, "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62", 4, "Pastel Vegano Zanahoria", 4, "Mediano" },
                    { 102, 5, 9, "https://images.unsplash.com/photo-1562440499-64c9a111f713", 1, "Especial 50 Años", 2, "Grande" },
                    { 105, 6, 1, "https://images.unsplash.com/photo-1519869325930-281384150729", 5, "Limón Cítrico", 6, "Chico" }
                });

            migrationBuilder.InsertData(
                table: "Pedidos",
                columns: new[] { "Id", "Cantidad", "Estatus", "Fecha", "PastelId", "UsuarioId" },
                values: new object[,]
                {
                    { 107, 2, "Entregado", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 104, 2 },
                    { 109, 4, "Pendiente", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 106, 2 },
                    { 105, 3, "Pendiente", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, 2 },
                    { 108, 1, "En Proceso", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 102, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CatCategorias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CatCategorias",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Mermas",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Pedidos",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CatCategorias",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CatCategorias",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "FotoUrl",
                value: "selva_negra.jpg");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "FotoUrl",
                value: "spiderman.jpg");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "FotoUrl",
                value: "rosca.jpg");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                column: "FotoUrl",
                value: "boda.jpg");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                column: "FotoUrl",
                value: "keto.jpg");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId" },
                values: new object[] { 1, "tres_leches.jpg", 2 });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nombre",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "Cliente");
        }
    }
}

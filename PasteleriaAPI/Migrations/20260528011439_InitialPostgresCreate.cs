using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatBizcochos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatBizcochos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoria = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatCategorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatGlaseados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatGlaseados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatRellenos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatRellenos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Rol = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pasteles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Tamanio = table.Column<string>(type: "text", nullable: false),
                    FotoUrl = table.Column<string>(type: "text", nullable: true),
                    IsPersonalizado = table.Column<bool>(type: "boolean", nullable: false),
                    BizcochoId = table.Column<int>(type: "integer", nullable: false),
                    RellenoId = table.Column<int>(type: "integer", nullable: false),
                    GlaseadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasteles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasteles_CatBizcochos_BizcochoId",
                        column: x => x.BizcochoId,
                        principalTable: "CatBizcochos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pasteles_CatCategorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CatCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pasteles_CatGlaseados_GlaseadoId",
                        column: x => x.GlaseadoId,
                        principalTable: "CatGlaseados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pasteles_CatRellenos_RellenoId",
                        column: x => x.RellenoId,
                        principalTable: "CatRellenos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mermas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PastelId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Estatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mermas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mermas_Pasteles_PastelId",
                        column: x => x.PastelId,
                        principalTable: "Pasteles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mermas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PastelId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Estatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Pasteles_PastelId",
                        column: x => x.PastelId,
                        principalTable: "Pasteles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CatBizcochos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Vainilla" },
                    { 2, "Chocolate" },
                    { 3, "Red Velvet" },
                    { 4, "Zanahoria" },
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
                    { 1, "Gourmet" },
                    { 2, "Infantil" },
                    { 3, "Boda" },
                    { 4, "Tradicional" },
                    { 5, "Dietético" },
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
                    { 1, "Fondant" },
                    { 2, "Crema de Mantequilla" },
                    { 3, "Ganache de Chocolate" },
                    { 4, "Queso Crema" },
                    { 5, "Merengue Italiano" },
                    { 6, "Espejo de Chocolate" },
                    { 7, "Chantilly" }
                });

            migrationBuilder.InsertData(
                table: "CatRellenos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fresa" },
                    { 2, "Cajeta" },
                    { 3, "Crema Pastelera" },
                    { 4, "Nutella" },
                    { 5, "Mermelada de Frambuesa" },
                    { 6, "Crema de Limón" },
                    { 7, "Durazno" },
                    { 8, "Frutos Rojos" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nombre", "Password", "Rol" },
                values: new object[,]
                {
                    { 1, "admin@pasteleria.com", "Admin Master", "admin", "Admin" },
                    { 2, "cliente@correo.com", "Cliente Juan", "123", "Cliente" }
                });

            migrationBuilder.InsertData(
                table: "Pasteles",
                columns: new[] { "Id", "BizcochoId", "CategoriaId", "FotoUrl", "GlaseadoId", "IsPersonalizado", "Nombre", "RellenoId", "Tamanio" },
                values: new object[,]
                {
                    { 1, 2, 1, "https://images.unsplash.com/photo-1578985545062-69928b1d9587", 3, false, "Selva Negra", 1, "Grande" },
                    { 2, 1, 2, "https://images.unsplash.com/photo-1557925923-33b251dc32b0", 1, false, "Pastel de Spiderman", 2, "Mediano" },
                    { 3, 1, 4, "https://images.unsplash.com/photo-1535141192574-5d4897c12636", 2, false, "Rosca de Reyes", 3, "Grande" },
                    { 4, 3, 3, "https://images.unsplash.com/photo-1535254973040-607b474cb50d", 4, false, "Pastel Nupcial Blanco", 4, "Extra Grande" },
                    { 5, 1, 5, "https://images.unsplash.com/photo-1533134242443-d4fd215305ad", 4, false, "Cheesecake Keto", 1, "Chico" },
                    { 6, 8, 4, "https://images.unsplash.com/photo-1464349095431-e9a21285b5f3", 7, false, "Tres Leches", 3, "Mediano" },
                    { 101, 4, 8, "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62", 4, false, "Pastel Vegano Zanahoria", 4, "Mediano" },
                    { 102, 5, 9, "https://images.unsplash.com/photo-1562440499-64c9a111f713", 1, false, "Especial 50 Años", 2, "Grande" },
                    { 103, 2, 1, "https://images.unsplash.com/photo-1550617931-e17a7b70dce2", 3, false, "Moka Intenso", 4, "Mediano" },
                    { 104, 1, 2, "https://images.unsplash.com/photo-1616541823729-00fe0aacd32c", 1, false, "Unicornio Mágico", 1, "Grande" },
                    { 105, 6, 1, "https://images.unsplash.com/photo-1519869325930-281384150729", 5, false, "Limón Cítrico", 6, "Chico" },
                    { 106, 2, 1, "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62", 3, false, "Chocolate Extremo", 4, "Extra Grande" }
                });

            migrationBuilder.InsertData(
                table: "Mermas",
                columns: new[] { "Id", "Descripcion", "Estatus", "Fecha", "PastelId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, "Pastel aplastado durante el transporte", "Pendiente", new DateTime(2023, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 2, "Falta de decoración en el cheesecake", "Pendiente", new DateTime(2023, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 },
                    { 103, "El cliente reportó que el chocolate estaba muy amargo", "Pendiente", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 106, 2 },
                    { 104, "Flores de fondant derretidas por el calor", "Pendiente", new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 105, "La leche se derramó en la caja", "Pendiente", new DateTime(2023, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2 }
                });

            migrationBuilder.InsertData(
                table: "Pedidos",
                columns: new[] { "Id", "Cantidad", "Estatus", "Fecha", "FechaEntrega", "PastelId", "UsuarioId" },
                values: new object[,]
                {
                    { 1, 2, "Entregado", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 2, 1, "En Proceso", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2 },
                    { 3, 5, "Pendiente", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2 },
                    { 104, 1, "Entregado", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 105, 3, "Pendiente", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, 2 },
                    { 106, 1, "Cancelado", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 107, 2, "Entregado", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104, 2 },
                    { 108, 1, "En Proceso", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102, 2 },
                    { 109, 4, "Pendiente", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106, 2 },
                    { 110, 2, "Entregado", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_PastelId",
                table: "Mermas",
                column: "PastelId");

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_UsuarioId",
                table: "Mermas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_BizcochoId",
                table: "Pasteles",
                column: "BizcochoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_CategoriaId",
                table: "Pasteles",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_GlaseadoId",
                table: "Pasteles",
                column: "GlaseadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_RellenoId",
                table: "Pasteles",
                column: "RellenoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_PastelId",
                table: "Pedidos",
                column: "PastelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mermas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Pasteles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "CatBizcochos");

            migrationBuilder.DropTable(
                name: "CatCategorias");

            migrationBuilder.DropTable(
                name: "CatGlaseados");

            migrationBuilder.DropTable(
                name: "CatRellenos");
        }
    }
}

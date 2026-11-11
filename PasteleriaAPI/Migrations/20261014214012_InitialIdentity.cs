using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatBizcochos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatBizcochos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    categoria = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatCategorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatGlaseados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatGlaseados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatRellenos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatRellenos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pasteles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Tamanio = table.Column<string>(type: "TEXT", nullable: false),
                    FotoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsPersonalizado = table.Column<bool>(type: "INTEGER", nullable: false),
                    BizcochoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RellenoId = table.Column<int>(type: "INTEGER", nullable: false),
                    GlaseadoId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PastelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<string>(type: "TEXT", nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true),
                    Estatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mermas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mermas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mermas_Pasteles_PastelId",
                        column: x => x.PastelId,
                        principalTable: "Pasteles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PastelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<string>(type: "TEXT", nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Estatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Pasteles_PastelId",
                        column: x => x.PastelId,
                        principalTable: "Pasteles",
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Mermas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pasteles");

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

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialPasteleria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatCategorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pasteles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sabor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tamanio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasteles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasteles_CatCategorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CatCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    { 5, "Dietético" }
                });

            migrationBuilder.InsertData(
                table: "Pasteles",
                columns: new[] { "Id", "CategoriaId", "Nombre", "Sabor", "Tamanio" },
                values: new object[,]
                {
                    { 1, 1, "Selva Negra", "Chocolate y Cereza", "Grande" },
                    { 2, 2, "Pastel de Spiderman", "Vainilla", "Mediano" },
                    { 3, 4, "Rosca de Reyes", "Naranja", "Grande" },
                    { 4, 3, "Pastel Nupcial Blanco", "Red Velvet", "Extra Grande" },
                    { 5, 5, "Cheesecake Keto", "Fresa Sin Azúcar", "Chico" },
                    { 6, 4, "Tres Leches", "Vainilla Tradicional", "Mediano" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_CategoriaId",
                table: "Pasteles",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pasteles");

            migrationBuilder.DropTable(
                name: "CatCategorias");
        }
    }
}

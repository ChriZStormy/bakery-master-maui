using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePastelCatalogos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sabor",
                table: "Pasteles");

            migrationBuilder.AddColumn<int>(
                name: "BizcochoId",
                table: "Pasteles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Pasteles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GlaseadoId",
                table: "Pasteles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RellenoId",
                table: "Pasteles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CatBizcochos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatBizcochos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatGlaseados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatGlaseados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatRellenos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatRellenos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CatBizcochos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Vainilla" },
                    { 2, "Chocolate" },
                    { 3, "Red Velvet" },
                    { 4, "Zanahoria" }
                });

            migrationBuilder.InsertData(
                table: "CatGlaseados",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fondant" },
                    { 2, "Crema de Mantequilla" },
                    { 3, "Ganache de Chocolate" },
                    { 4, "Queso Crema" }
                });

            migrationBuilder.InsertData(
                table: "CatRellenos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fresa" },
                    { 2, "Cajeta" },
                    { 3, "Crema Pastelera" },
                    { 4, "Nutella" }
                });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 2, "selva_negra.jpg", 3, 1 });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 1, "spiderman.jpg", 1, 2 });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 1, "rosca.jpg", 2, 3 });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 3, "boda.jpg", 4, 4 });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 1, "keto.jpg", 4, 1 });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BizcochoId", "FotoUrl", "GlaseadoId", "RellenoId" },
                values: new object[] { 1, "tres_leches.jpg", 2, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_BizcochoId",
                table: "Pasteles",
                column: "BizcochoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_GlaseadoId",
                table: "Pasteles",
                column: "GlaseadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasteles_RellenoId",
                table: "Pasteles",
                column: "RellenoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pasteles_CatBizcochos_BizcochoId",
                table: "Pasteles",
                column: "BizcochoId",
                principalTable: "CatBizcochos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pasteles_CatGlaseados_GlaseadoId",
                table: "Pasteles",
                column: "GlaseadoId",
                principalTable: "CatGlaseados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pasteles_CatRellenos_RellenoId",
                table: "Pasteles",
                column: "RellenoId",
                principalTable: "CatRellenos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pasteles_CatBizcochos_BizcochoId",
                table: "Pasteles");

            migrationBuilder.DropForeignKey(
                name: "FK_Pasteles_CatGlaseados_GlaseadoId",
                table: "Pasteles");

            migrationBuilder.DropForeignKey(
                name: "FK_Pasteles_CatRellenos_RellenoId",
                table: "Pasteles");

            migrationBuilder.DropTable(
                name: "CatBizcochos");

            migrationBuilder.DropTable(
                name: "CatGlaseados");

            migrationBuilder.DropTable(
                name: "CatRellenos");

            migrationBuilder.DropIndex(
                name: "IX_Pasteles_BizcochoId",
                table: "Pasteles");

            migrationBuilder.DropIndex(
                name: "IX_Pasteles_GlaseadoId",
                table: "Pasteles");

            migrationBuilder.DropIndex(
                name: "IX_Pasteles_RellenoId",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "BizcochoId",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "GlaseadoId",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "RellenoId",
                table: "Pasteles");

            migrationBuilder.AddColumn<string>(
                name: "Sabor",
                table: "Pasteles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Sabor",
                value: "Chocolate y Cereza");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Sabor",
                value: "Vainilla");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Sabor",
                value: "Naranja");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Sabor",
                value: "Red Velvet");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Sabor",
                value: "Fresa Sin Azúcar");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Sabor",
                value: "Vainilla Tradicional");
        }
    }
}

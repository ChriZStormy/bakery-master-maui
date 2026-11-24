using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Sprint1_CatalogoYFichaTecnica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alergenos",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotosUrls",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ingredientes",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioBase",
                table: "Pasteles",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "CatRellenos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Agotado",
                table: "CatRellenos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoAdicional",
                table: "CatRellenos",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "CatGlaseados",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Agotado",
                table: "CatGlaseados",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoAdicional",
                table: "CatGlaseados",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "CatBizcochos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Agotado",
                table: "CatBizcochos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoAdicional",
                table: "CatBizcochos",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Activo", "Agotado", "CostoAdicional" },
                values: new object[] { true, false, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "Alergenos", "Descripcion", "FotosUrls", "Ingredientes", "PrecioBase" },
                values: new object[] { null, null, null, null, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alergenos",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "FotosUrls",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "Ingredientes",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "PrecioBase",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "CatRellenos");

            migrationBuilder.DropColumn(
                name: "Agotado",
                table: "CatRellenos");

            migrationBuilder.DropColumn(
                name: "CostoAdicional",
                table: "CatRellenos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "CatGlaseados");

            migrationBuilder.DropColumn(
                name: "Agotado",
                table: "CatGlaseados");

            migrationBuilder.DropColumn(
                name: "CostoAdicional",
                table: "CatGlaseados");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "CatBizcochos");

            migrationBuilder.DropColumn(
                name: "Agotado",
                table: "CatBizcochos");

            migrationBuilder.DropColumn(
                name: "CostoAdicional",
                table: "CatBizcochos");
        }
    }
}

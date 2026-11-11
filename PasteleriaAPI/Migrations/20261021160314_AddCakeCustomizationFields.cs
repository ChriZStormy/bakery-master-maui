using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasteleriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCakeCustomizationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenDecoracion",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mensaje",
                table: "Pedidos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenDecoracion",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mensaje",
                table: "Pasteles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "CatRellenos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "CatGlaseados",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "CatBizcochos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenUrl",
                value: "bizcocho_vainilla.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenUrl",
                value: "bizcocho_chocolate.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenUrl",
                value: "bizcocho_red_velvet.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenUrl",
                value: "bizcocho_zanahoria.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenUrl",
                value: "bizcocho_marmol.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagenUrl",
                value: "bizcocho_limon.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagenUrl",
                value: "bizcocho_naranja.png");

            migrationBuilder.UpdateData(
                table: "CatBizcochos",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagenUrl",
                value: "bizcocho_esponja.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenUrl",
                value: "glaseado_fondant.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenUrl",
                value: "glaseado_crema_mantequilla.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenUrl",
                value: "glaseado_ganache_chocolate.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenUrl",
                value: "glaseado_queso_crema.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenUrl",
                value: "glaseado_merengue_italiano.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagenUrl",
                value: "glaseado_espejo_chocolate.png");

            migrationBuilder.UpdateData(
                table: "CatGlaseados",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagenUrl",
                value: "glaseado_chantilly.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenUrl",
                value: "relleno_fresa.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenUrl",
                value: "relleno_cajeta.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenUrl",
                value: "relleno_crema_pastelera.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenUrl",
                value: "relleno_nutella.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenUrl",
                value: "relleno_mermelada_frambuesa.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagenUrl",
                value: "relleno_crema_limon.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagenUrl",
                value: "relleno_durazno.png");

            migrationBuilder.UpdateData(
                table: "CatRellenos",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagenUrl",
                value: "relleno_frutos_rojos.png");

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Pasteles",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "ImagenDecoracion", "Mensaje" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenDecoracion",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Mensaje",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ImagenDecoracion",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "Mensaje",
                table: "Pasteles");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "CatRellenos");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "CatGlaseados");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "CatBizcochos");
        }
    }
}

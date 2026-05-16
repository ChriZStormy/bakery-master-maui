using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;

namespace PasteleriaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatCategoria>().HasData(
                new CatCategoria { Id = 1, categoria = "Gourmet" },
                new CatCategoria { Id = 2, categoria = "Infantil" },
                new CatCategoria { Id = 3, categoria = "Boda" },
                new CatCategoria { Id = 4, categoria = "Tradicional" },
                new CatCategoria { Id = 5, categoria = "Dietético" },
                new CatCategoria { Id = 6, categoria = "Quinceañera" },
                new CatCategoria { Id = 7, categoria = "Temático" },
                new CatCategoria { Id = 8, categoria = "Vegano" },
                new CatCategoria { Id = 9, categoria = "Aniversario" }
            );

            modelBuilder.Entity<CatBizcocho>().HasData(
                new CatBizcocho { Id = 1, Nombre = "Vainilla" },
                new CatBizcocho { Id = 2, Nombre = "Chocolate" },
                new CatBizcocho { Id = 3, Nombre = "Red Velvet" },
                new CatBizcocho { Id = 4, Nombre = "Zanahoria" },
                new CatBizcocho { Id = 5, Nombre = "Mármol" },
                new CatBizcocho { Id = 6, Nombre = "Limón" },
                new CatBizcocho { Id = 7, Nombre = "Naranja" },
                new CatBizcocho { Id = 8, Nombre = "Esponja" }
            );

            modelBuilder.Entity<CatRelleno>().HasData(
                new CatRelleno { Id = 1, Nombre = "Fresa" },
                new CatRelleno { Id = 2, Nombre = "Cajeta" },
                new CatRelleno { Id = 3, Nombre = "Crema Pastelera" },
                new CatRelleno { Id = 4, Nombre = "Nutella" },
                new CatRelleno { Id = 5, Nombre = "Mermelada de Frambuesa" },
                new CatRelleno { Id = 6, Nombre = "Crema de Limón" },
                new CatRelleno { Id = 7, Nombre = "Durazno" },
                new CatRelleno { Id = 8, Nombre = "Frutos Rojos" }
            );

            modelBuilder.Entity<CatGlaseado>().HasData(
                new CatGlaseado { Id = 1, Nombre = "Fondant" },
                new CatGlaseado { Id = 2, Nombre = "Crema de Mantequilla" },
                new CatGlaseado { Id = 3, Nombre = "Ganache de Chocolate" },
                new CatGlaseado { Id = 4, Nombre = "Queso Crema" },
                new CatGlaseado { Id = 5, Nombre = "Merengue Italiano" },
                new CatGlaseado { Id = 6, Nombre = "Espejo de Chocolate" },
                new CatGlaseado { Id = 7, Nombre = "Chantilly" }
            );

            modelBuilder.Entity<Pastel>().HasData(
                new Pastel { Id = 1, Nombre = "Selva Negra", BizcochoId = 2, RellenoId = 1, GlaseadoId = 3, Tamanio = "Grande", CategoriaId = 1, FotoUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587" },
                new Pastel { Id = 2, Nombre = "Pastel de Spiderman", BizcochoId = 1, RellenoId = 2, GlaseadoId = 1, Tamanio = "Mediano", CategoriaId = 2, FotoUrl = "https://images.unsplash.com/photo-1557925923-33b251dc32b0" },
                new Pastel { Id = 3, Nombre = "Rosca de Reyes", BizcochoId = 1, RellenoId = 3, GlaseadoId = 2, Tamanio = "Grande", CategoriaId = 4, FotoUrl = "https://images.unsplash.com/photo-1535141192574-5d4897c12636" },
                new Pastel { Id = 4, Nombre = "Pastel Nupcial Blanco", BizcochoId = 3, RellenoId = 4, GlaseadoId = 4, Tamanio = "Extra Grande", CategoriaId = 3, FotoUrl = "https://images.unsplash.com/photo-1535254973040-607b474cb50d" },
                new Pastel { Id = 5, Nombre = "Cheesecake Keto", BizcochoId = 1, RellenoId = 1, GlaseadoId = 4, Tamanio = "Chico", CategoriaId = 5, FotoUrl = "https://images.unsplash.com/photo-1533134242443-d4fd215305ad" },
                new Pastel { Id = 6, Nombre = "Tres Leches", BizcochoId = 8, RellenoId = 3, GlaseadoId = 7, Tamanio = "Mediano", CategoriaId = 4, FotoUrl = "https://images.unsplash.com/photo-1464349095431-e9a21285b5f3" },
                new Pastel { Id = 101, Nombre = "Pastel Vegano Zanahoria", BizcochoId = 4, RellenoId = 4, GlaseadoId = 4, Tamanio = "Mediano", CategoriaId = 8, FotoUrl = "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62" },
                new Pastel { Id = 102, Nombre = "Especial 50 Años", BizcochoId = 5, RellenoId = 2, GlaseadoId = 1, Tamanio = "Grande", CategoriaId = 9, FotoUrl = "https://images.unsplash.com/photo-1562440499-64c9a111f713" },
                new Pastel { Id = 103, Nombre = "Moka Intenso", BizcochoId = 2, RellenoId = 4, GlaseadoId = 3, Tamanio = "Mediano", CategoriaId = 1, FotoUrl = "https://images.unsplash.com/photo-1550617931-e17a7b70dce2" },
                new Pastel { Id = 104, Nombre = "Unicornio Mágico", BizcochoId = 1, RellenoId = 1, GlaseadoId = 1, Tamanio = "Grande", CategoriaId = 2, FotoUrl = "https://images.unsplash.com/photo-1616541823729-00fe0aacd32c" },
                new Pastel { Id = 105, Nombre = "Limón Cítrico", BizcochoId = 6, RellenoId = 6, GlaseadoId = 5, Tamanio = "Chico", CategoriaId = 1, FotoUrl = "https://images.unsplash.com/photo-1519869325930-281384150729" },
                new Pastel { Id = 106, Nombre = "Chocolate Extremo", BizcochoId = 2, RellenoId = 4, GlaseadoId = 3, Tamanio = "Extra Grande", CategoriaId = 1, FotoUrl = "https://images.unsplash.com/photo-1606890737304-57a1ca8a5b62" }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Admin Master", Email = "admin@pasteleria.com", Password = "admin", Rol = "Admin" },
                new Usuario { Id = 2, Nombre = "Cliente Juan", Email = "cliente@correo.com", Password = "123", Rol = "Cliente" }
            );

            modelBuilder.Entity<Pedido>().HasData(
                new Pedido { Id = 1, PastelId = 1, UsuarioId = 2, Fecha = new DateTime(2023, 10, 1), Cantidad = 2, Estatus = "Entregado" },
                new Pedido { Id = 2, PastelId = 3, UsuarioId = 2, Fecha = new DateTime(2023, 10, 5), Cantidad = 1, Estatus = "En Proceso" },
                new Pedido { Id = 3, PastelId = 6, UsuarioId = 2, Fecha = new DateTime(2023, 10, 10), Cantidad = 5, Estatus = "Pendiente" },
                new Pedido { Id = 104, PastelId = 4, UsuarioId = 2, Fecha = new DateTime(2023, 11, 15), Cantidad = 1, Estatus = "Entregado" },
                new Pedido { Id = 105, PastelId = 101, UsuarioId = 2, Fecha = new DateTime(2023, 11, 20), Cantidad = 3, Estatus = "Pendiente" },
                new Pedido { Id = 106, PastelId = 2, UsuarioId = 2, Fecha = new DateTime(2023, 12, 02), Cantidad = 1, Estatus = "Cancelado" },
                new Pedido { Id = 107, PastelId = 104, UsuarioId = 2, Fecha = new DateTime(2023, 12, 12), Cantidad = 2, Estatus = "Entregado" },
                new Pedido { Id = 108, PastelId = 102, UsuarioId = 2, Fecha = new DateTime(2024, 01, 10), Cantidad = 1, Estatus = "En Proceso" },
                new Pedido { Id = 109, PastelId = 106, UsuarioId = 2, Fecha = new DateTime(2024, 02, 14), Cantidad = 4, Estatus = "Pendiente" },
                new Pedido { Id = 110, PastelId = 5, UsuarioId = 2, Fecha = new DateTime(2024, 03, 01), Cantidad = 2, Estatus = "Entregado" }
            );

            modelBuilder.Entity<Merma>().HasData(
                new Merma { Id = 1, PastelId = 2, UsuarioId = 2, Fecha = new DateTime(2023, 10, 2), Descripcion = "Pastel aplastado durante el transporte" },
                new Merma { Id = 2, PastelId = 5, UsuarioId = 2, Fecha = new DateTime(2023, 10, 6), Descripcion = "Falta de decoración en el cheesecake" },
                new Merma { Id = 103, PastelId = 106, UsuarioId = 2, Fecha = new DateTime(2024, 02, 15), Descripcion = "El cliente reportó que el chocolate estaba muy amargo" },
                new Merma { Id = 104, PastelId = 4, UsuarioId = 2, Fecha = new DateTime(2023, 11, 16), Descripcion = "Flores de fondant derretidas por el calor" },
                new Merma { Id = 105, PastelId = 6, UsuarioId = 2, Fecha = new DateTime(2023, 10, 11), Descripcion = "La leche se derramó en la caja" }
            );
        }

        public DbSet<CatCategoria> CatCategorias { get; set; }
        public DbSet<CatBizcocho> CatBizcochos { get; set; }
        public DbSet<CatRelleno> CatRellenos { get; set; }
        public DbSet<CatGlaseado> CatGlaseados { get; set; }
        public DbSet<Pastel> Pasteles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Merma> Mermas { get; set; }
    }
}

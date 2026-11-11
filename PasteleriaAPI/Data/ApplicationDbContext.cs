using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;

namespace PasteleriaAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
                new CatBizcocho { Id = 1, Nombre = "Vainilla", ImagenUrl = "bizcocho_vainilla.png" },
                new CatBizcocho { Id = 2, Nombre = "Chocolate", ImagenUrl = "bizcocho_chocolate.png" },
                new CatBizcocho { Id = 3, Nombre = "Red Velvet", ImagenUrl = "bizcocho_red_velvet.png" },
                new CatBizcocho { Id = 4, Nombre = "Zanahoria", ImagenUrl = "bizcocho_zanahoria.png" },
                new CatBizcocho { Id = 5, Nombre = "Mármol", ImagenUrl = "bizcocho_marmol.png" },
                new CatBizcocho { Id = 6, Nombre = "Limón", ImagenUrl = "bizcocho_limon.png" },
                new CatBizcocho { Id = 7, Nombre = "Naranja", ImagenUrl = "bizcocho_naranja.png" },
                new CatBizcocho { Id = 8, Nombre = "Esponja", ImagenUrl = "bizcocho_esponja.png" }
            );

            modelBuilder.Entity<CatRelleno>().HasData(
                new CatRelleno { Id = 1, Nombre = "Fresa", ImagenUrl = "relleno_fresa.png" },
                new CatRelleno { Id = 2, Nombre = "Cajeta", ImagenUrl = "relleno_cajeta.png" },
                new CatRelleno { Id = 3, Nombre = "Crema Pastelera", ImagenUrl = "relleno_crema_pastelera.png" },
                new CatRelleno { Id = 4, Nombre = "Nutella", ImagenUrl = "relleno_nutella.png" },
                new CatRelleno { Id = 5, Nombre = "Mermelada de Frambuesa", ImagenUrl = "relleno_mermelada_frambuesa.png" },
                new CatRelleno { Id = 6, Nombre = "Crema de Limón", ImagenUrl = "relleno_crema_limon.png" },
                new CatRelleno { Id = 7, Nombre = "Durazno", ImagenUrl = "relleno_durazno.png" },
                new CatRelleno { Id = 8, Nombre = "Frutos Rojos", ImagenUrl = "relleno_frutos_rojos.png" }
            );

            modelBuilder.Entity<CatGlaseado>().HasData(
                new CatGlaseado { Id = 1, Nombre = "Fondant", ImagenUrl = "glaseado_fondant.png" },
                new CatGlaseado { Id = 2, Nombre = "Crema de Mantequilla", ImagenUrl = "glaseado_crema_mantequilla.png" },
                new CatGlaseado { Id = 3, Nombre = "Ganache de Chocolate", ImagenUrl = "glaseado_ganache_chocolate.png" },
                new CatGlaseado { Id = 4, Nombre = "Queso Crema", ImagenUrl = "glaseado_queso_crema.png" },
                new CatGlaseado { Id = 5, Nombre = "Merengue Italiano", ImagenUrl = "glaseado_merengue_italiano.png" },
                new CatGlaseado { Id = 6, Nombre = "Espejo de Chocolate", ImagenUrl = "glaseado_espejo_chocolate.png" },
                new CatGlaseado { Id = 7, Nombre = "Chantilly", ImagenUrl = "glaseado_chantilly.png" }
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
        }

        public DbSet<CatCategoria> CatCategorias { get; set; }
        public DbSet<CatBizcocho> CatBizcochos { get; set; }
        public DbSet<CatRelleno> CatRellenos { get; set; }
        public DbSet<CatGlaseado> CatGlaseados { get; set; }
        public DbSet<Pastel> Pasteles { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Merma> Mermas { get; set; }
    }
}

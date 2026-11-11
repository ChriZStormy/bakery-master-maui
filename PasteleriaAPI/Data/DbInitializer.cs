using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Create roles
            string[] roles = { "Admin", "Cliente", "Repostero", "Empleado" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Rol '{Role}' creado exitosamente.", role);
                }
            }

            // Asegurar que los usuarios existentes tengan Activo = true
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.ExecuteSqlRawAsync("UPDATE AspNetUsers SET Activo = 1 WHERE Activo = 0;");

            // Sembrado de ImagenUrl para catálogos modulares
            var bizcochoImages = new Dictionary<int, string>
            {
                { 1, "bizcocho_vainilla.png" }, { 2, "bizcocho_chocolate.png" },
                { 3, "bizcocho_red_velvet.png" }, { 4, "bizcocho_zanahoria.png" },
                { 5, "bizcocho_marmol.png" }, { 6, "bizcocho_limon.png" },
                { 7, "bizcocho_naranja.png" }, { 8, "bizcocho_esponja.png" }
            };
            foreach (var kvp in bizcochoImages)
            {
                var item = await dbContext.CatBizcochos.FindAsync(kvp.Key);
                if (item != null) item.ImagenUrl = kvp.Value;
            }

            var rellenoImages = new Dictionary<int, string>
            {
                { 1, "relleno_fresa.png" }, { 2, "relleno_cajeta.png" },
                { 3, "relleno_crema_pastelera.png" }, { 4, "relleno_nutella.png" },
                { 5, "relleno_mermelada_frambuesa.png" }, { 6, "relleno_crema_limon.png" },
                { 7, "relleno_durazno.png" }, { 8, "relleno_frutos_rojos.png" }
            };
            foreach (var kvp in rellenoImages)
            {
                var item = await dbContext.CatRellenos.FindAsync(kvp.Key);
                if (item != null) item.ImagenUrl = kvp.Value;
            }

            var glaseadoImages = new Dictionary<int, string>
            {
                { 1, "glaseado_fondant.png" }, { 2, "glaseado_crema_mantequilla.png" },
                { 3, "glaseado_ganache_chocolate.png" }, { 4, "glaseado_queso_crema.png" },
                { 5, "glaseado_merengue_italiano.png" }, { 6, "glaseado_espejo_chocolate.png" },
                { 7, "glaseado_chantilly.png" }
            };
            foreach (var kvp in glaseadoImages)
            {
                var item = await dbContext.CatGlaseados.FindAsync(kvp.Key);
                if (item != null) item.ImagenUrl = kvp.Value;
            }
            await dbContext.SaveChangesAsync();

            // Create master admin user
            var adminEmail = configuration["AdminSeed:Email"] ?? "admin@pasteleria.com";
            var adminPassword = configuration["AdminSeed:Password"] ?? "Admin@123456";
            var adminName = configuration["AdminSeed:Nombre"] ?? "Admin Master";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = adminName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Usuario administrador maestro creado: {Email}", adminEmail);
                }
                else
                {
                    logger.LogError("Error al crear admin maestro: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

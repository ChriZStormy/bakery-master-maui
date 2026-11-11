using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;
using System.Security.Claims;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MermaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public MermaController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Merma>>> GetMermas(
            [FromQuery] string? usuarioId,
            [FromQuery] string? causa,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var isStaff = User.IsInRole("Repostero") || User.IsInRole("Empleado");

            if (!isAdmin && !isStaff)
            {
                // Seguridad IDOR: Los clientes solo pueden consultar sus propios reclamos/mermas
                usuarioId = currentUserId;
            }
            else if (isAdmin)
            {
                // Si un Admin consulta registros globales, valida la política de red local si aplica
                var authResult = await _authorizationService.AuthorizeAsync(User, "LocalNetworkOnly");
                if (!authResult.Succeeded)
                {
                    return Forbid();
                }
            }

            var query = _context.Mermas
                .Include(m => m.Pastel)
                .Include(m => m.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(usuarioId))
            {
                query = query.Where(m => m.UsuarioId == usuarioId);
            }

            if (!string.IsNullOrEmpty(causa) && causa != "Todas" && causa != "Todos")
            {
                query = query.Where(m => m.Causa == causa);
            }

            if (fechaInicio.HasValue)
            {
                var inicio = fechaInicio.Value.Date;
                query = query.Where(m => m.Fecha >= inicio);
            }

            if (fechaFin.HasValue)
            {
                var fin = fechaFin.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(m => m.Fecha <= fin);
            }

            return await query.OrderByDescending(m => m.Fecha).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Merma>> ReportarMerma([FromBody] Merma merma)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            if (merma.Fecha == default)
            {
                merma.Fecha = DateTime.Now;
            }

            if (string.IsNullOrEmpty(merma.Estatus))
            {
                merma.Estatus = "Registrada";
            }

            // Seguridad IDOR: Forzar el ID del usuario autenticado del token JWT
            merma.UsuarioId = currentUserId;

            // Si no se ingresó costo estimado, calcularlo automáticamente según el elemento afectado
            decimal qty = (decimal)(merma.Cantidad > 0 ? merma.Cantidad : 1);
            if (merma.PastelId.HasValue && merma.PastelId.Value > 0)
            {
                var pastel = await _context.Pasteles.FindAsync(merma.PastelId.Value);
                if (pastel != null)
                {
                    if (string.IsNullOrWhiteSpace(merma.ElementoAfectado))
                    {
                        merma.ElementoAfectado = pastel.Nombre;
                    }

                    if (merma.CostoEstimado <= 0)
                    {
                        decimal precioUnitario = pastel.Tamanio?.ToLowerInvariant() switch
                        {
                            "chico" => 250m,
                            "mediano" => 380m,
                            "grande" => 520m,
                            "extra grande" => 680m,
                            _ => 380m
                        };
                        merma.CostoEstimado = precioUnitario * qty;
                    }
                }
            }
            else if (merma.CostoEstimado <= 0)
            {
                // Insumo / materia prima fallback
                string elem = merma.ElementoAfectado ?? string.Empty;
                decimal costoUnitario = 50.00m;
                if (elem.Contains("Harina", StringComparison.OrdinalIgnoreCase)) costoUnitario = 28.00m;
                else if (elem.Contains("Azúcar", StringComparison.OrdinalIgnoreCase) || elem.Contains("Azucar", StringComparison.OrdinalIgnoreCase)) costoUnitario = 35.00m;
                else if (elem.Contains("Mantequilla", StringComparison.OrdinalIgnoreCase)) costoUnitario = 140.00m;
                else if (elem.Contains("Huevo", StringComparison.OrdinalIgnoreCase)) costoUnitario = 4.50m;
                else if (elem.Contains("Leche", StringComparison.OrdinalIgnoreCase)) costoUnitario = 26.00m;
                else if (elem.Contains("Crema", StringComparison.OrdinalIgnoreCase) || elem.Contains("Chantilly", StringComparison.OrdinalIgnoreCase)) costoUnitario = 85.00m;
                else if (elem.Contains("Chocolate", StringComparison.OrdinalIgnoreCase)) costoUnitario = 180.00m;
                else if (elem.Contains("Fruto", StringComparison.OrdinalIgnoreCase) || elem.Contains("Fresa", StringComparison.OrdinalIgnoreCase)) costoUnitario = 120.00m;
                else if (elem.Contains("Bizcocho", StringComparison.OrdinalIgnoreCase)) costoUnitario = 110.00m;
                else if (elem.Contains("Relleno", StringComparison.OrdinalIgnoreCase)) costoUnitario = 90.00m;
                else if (elem.Contains("Glaseado", StringComparison.OrdinalIgnoreCase)) costoUnitario = 75.00m;

                merma.CostoEstimado = costoUnitario * qty;
            }

            if (string.IsNullOrWhiteSpace(merma.ElementoAfectado))
            {
                merma.ElementoAfectado = "Insumo / Pastel general";
            }

            // Evitar re-inserción de propiedades de navegación anidadas
            merma.Pastel = null;
            merma.Usuario = null;

            _context.Mermas.Add(merma);
            await _context.SaveChangesAsync();
            return Ok(merma);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", Policy = "LocalNetworkOnly")]
        public async Task<IActionResult> ActualizarMerma(int id, [FromBody] Merma mermaActualizada)
        {
            var merma = await _context.Mermas.FindAsync(id);
            if (merma == null) return NotFound();

            if (!string.IsNullOrEmpty(mermaActualizada.Estatus))
            {
                merma.Estatus = mermaActualizada.Estatus;
            }

            if (!string.IsNullOrEmpty(mermaActualizada.Descripcion))
            {
                merma.Descripcion = mermaActualizada.Descripcion;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

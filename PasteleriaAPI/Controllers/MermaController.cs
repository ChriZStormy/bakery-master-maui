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
        public async Task<ActionResult<List<Merma>>> GetMermas([FromQuery] string? usuarioId)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                // Seguridad IDOR: Los clientes solo pueden consultar sus propios reclamos/mermas
                usuarioId = currentUserId;
            }
            else
            {
                // Tarea 2: Si un Admin consulta registros globales, debe cumplir la política LocalNetworkOnly
                var authResult = await _authorizationService.AuthorizeAsync(User, "LocalNetworkOnly");
                if (!authResult.Succeeded)
                {
                    return Forbid();
                }
            }

            var query = _context.Mermas.Include(m => m.Pastel).Include(m => m.Usuario).AsQueryable();
            if (!string.IsNullOrEmpty(usuarioId))
            {
                query = query.Where(m => m.UsuarioId == usuarioId);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Merma>> ReportarMerma([FromBody] Merma merma)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            merma.Fecha = DateTime.Now;
            merma.Estatus = "Pendiente";

            // Seguridad IDOR: Forzar el ID del usuario autenticado del token JWT
            merma.UsuarioId = currentUserId;

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

            merma.Estatus = mermaActualizada.Estatus;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

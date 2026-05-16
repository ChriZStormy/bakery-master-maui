using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MermaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MermaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Merma>>> GetMermas([FromQuery] int? usuarioId)
        {
            var query = _context.Mermas.Include(m => m.Pastel).Include(m => m.Usuario).AsQueryable();
            if (usuarioId.HasValue)
            {
                query = query.Where(m => m.UsuarioId == usuarioId.Value);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Merma>> ReportarMerma([FromBody] Merma merma)
        {
            merma.Fecha = DateTime.Now;
            merma.Estatus = "Pendiente";
            _context.Mermas.Add(merma);
            await _context.SaveChangesAsync();
            return Ok(merma);
        }

        [HttpPut("{id}")]
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

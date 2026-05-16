using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> GetPedidos([FromQuery] int? usuarioId)
        {
            var query = _context.Pedidos
                .Include(p => p.Pastel).ThenInclude(p => p.Bizcocho)
                .Include(p => p.Pastel).ThenInclude(p => p.Relleno)
                .Include(p => p.Pastel).ThenInclude(p => p.Glaseado)
                .Include(p => p.Pastel).ThenInclude(p => p.Categoria)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(p => p.UsuarioId == usuarioId.Value);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido([FromBody] Pedido pedido)
        {
            pedido.Fecha = DateTime.Now;
            pedido.Estatus = "Pendiente";
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return Ok(pedido);
        }

        public class PedidoUpdateDto
        {
            public string Estatus { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedido(int id, [FromBody] PedidoUpdateDto pedidoActualizado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            pedido.Estatus = pedidoActualizado.Estatus;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

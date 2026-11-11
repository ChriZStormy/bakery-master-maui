using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Hubs;
using PasteleriaAPI.Services;
using System.Security.Claims;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        private readonly IHubContext<PedidoHub> _hubContext;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService,
            IEmailService emailService,
            IHubContext<PedidoHub> hubContext,
            ILogger<PedidoController> logger)
        {
            _context = context;
            _authorizationService = authorizationService;
            _emailService = emailService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("test-email")]
        public async Task<IActionResult> TestEmail([FromQuery] string destinatario)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                return BadRequest(new { 
                    exito = false, 
                    mensaje = "Por favor indica el correo de destino, ejemplo: /api/pedido/test-email?destinatario=tu_correo@gmail.com" 
                });
            }

            try
            {
                var htmlPrueba = @"
                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #F7F4EF;'>
                        <div style='background-color: #FFFFFF; padding: 25px; border-radius: 12px; border: 1px solid #8B5E34; max-width: 500px; margin: 0 auto;'>
                            <h2 style='color: #8B5E34; margin-top: 0;'>🎂 Dolce Vita Pastelería</h2>
                            <p style='color: #3E2723; font-size: 15px;'>¡Felicidades! Si estás viendo este mensaje, la conexión SMTP con <strong>Mailtrap Sandbox</strong> está funcionando al 100%.</p>
                            <hr style='border: none; border-top: 1px solid #EAE4DB; margin: 20px 0;'/>
                            <p style='font-size: 12px; color: #8D6E63;'>Enviado automáticamente desde el servicio transaccional de PasteleriaAPI a tu bandeja segura de Mailtrap.</p>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(destinatario, "Prueba Mailtrap Sandbox - Dolce Vita Pastelería", htmlPrueba, isHtml: true);
                
                return Ok(new { 
                    exito = true, 
                    mensaje = $"Correo de prueba enviado con éxito a '{destinatario}'. Revisa tu bandeja de entrada en Mailtrap Sandbox.",
                    servidor = "sandbox.smtp.mailtrap.io:2525"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo de prueba a {Destinatario}", destinatario);
                return StatusCode(500, new { 
                    exito = false, 
                    error = ex.Message, 
                    detalles = ex.ToString() 
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> GetPedidos([FromQuery] string? usuarioId)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var isRepostero = User.IsInRole("Repostero") || User.IsInRole("Empleado");

            if (!isAdmin && !isRepostero)
            {
                // Seguridad IDOR: Los clientes solo pueden consultar sus propios pedidos
                usuarioId = currentUserId;
            }
            else if (isAdmin)
            {
                // Tarea 2: Si un Admin consulta registros globales o de otros usuarios, debe cumplir la política LocalNetworkOnly
                var authResult = await _authorizationService.AuthorizeAsync(User, "LocalNetworkOnly");
                if (!authResult.Succeeded)
                {
                    return Forbid();
                }
            }
            // Repostero y Empleado pueden consultar todos los pedidos de producción sin restricción de IDOR

            var query = _context.Pedidos
                .Include(p => p.Pastel).ThenInclude(p => p.Bizcocho)
                .Include(p => p.Pastel).ThenInclude(p => p.Relleno)
                .Include(p => p.Pastel).ThenInclude(p => p.Glaseado)
                .Include(p => p.Pastel).ThenInclude(p => p.Categoria)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(usuarioId))
            {
                query = query.Where(p => p.UsuarioId == usuarioId);
            }
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido([FromBody] Pedido pedido)
        {
            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            pedido.Fecha = DateTime.Now;
            pedido.Estatus = "Pendiente";
            
            // Seguridad IDOR: Forzar el ID del usuario autenticado del token JWT
            pedido.UsuarioId = currentUserId;

            // Evitar re-inserción de propiedades de navegación anidadas
            pedido.Pastel = null;
            pedido.Usuario = null;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var pedidoCreado = await _context.Pedidos
                .Include(p => p.Pastel).ThenInclude(p => p.Bizcocho)
                .Include(p => p.Pastel).ThenInclude(p => p.Relleno)
                .Include(p => p.Pastel).ThenInclude(p => p.Glaseado)
                .Include(p => p.Pastel).ThenInclude(p => p.Categoria)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            // HU-05: Envío automático de comprobante HTML al correo del cliente
            var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value 
                ?? User.FindFirst("email")?.Value 
                ?? pedidoCreado?.Usuario?.Email;

            if (!string.IsNullOrEmpty(emailUsuario))
            {
                try
                {
                    await _emailService.EnviarComprobantePedidoAsync(emailUsuario, pedidoCreado ?? pedido);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al enviar comprobante de orden #{PedidoId} a {Email}", pedido.Id, emailUsuario);
                }
            }

            // HU-06: Notificar nuevo pedido a través del Hub de SignalR
            try
            {
                await _hubContext.Clients.All.SendAsync("NuevoPedidoRegistrado", pedido.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al notificar nuevo pedido #{PedidoId} por SignalR", pedido.Id);
            }

            return Ok(pedidoCreado ?? pedido);
        }

        public class PedidoUpdateDto
        {
            public string? Estatus { get; set; }
            public string? Notas { get; set; }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Repostero,Empleado")]
        public async Task<IActionResult> ActualizarPedido(int id, [FromBody] PedidoUpdateDto pedidoActualizado)
        {
            if (User.IsInRole("Admin"))
            {
                var authResult = await _authorizationService.AuthorizeAsync(User, "LocalNetworkOnly");
                if (!authResult.Succeeded)
                {
                    return Forbid();
                }
            }

            var pedido = await _context.Pedidos.Include(p => p.Usuario).FirstOrDefaultAsync(p => p.Id == id);
            if (pedido == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(pedidoActualizado.Estatus))
            {
                pedido.Estatus = pedidoActualizado.Estatus;
            }
            if (pedidoActualizado.Notas != null)
            {
                pedido.Notas = pedidoActualizado.Notas;
            }

            await _context.SaveChangesAsync();

            // HU-06: Emitir evento SignalR en tiempo real para actualización de estatus
            try
            {
                await _hubContext.Clients.All.SendAsync("PedidoEstatusActualizado", pedido.Id, pedido.Estatus);
                await _hubContext.Clients.All.SendAsync("EstatusPedidoCambiado", pedido.Id, pedido.Estatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al emitir evento SignalR de estatus para pedido #{PedidoId}", pedido.Id);
            }

            // Trigger Email Notification (HTML responsivo)
            if (pedido.Usuario != null && !string.IsNullOrWhiteSpace(pedido.Usuario.Email))
            {
                try
                {
                    await _emailService.EnviarActualizacionEstatusAsync(pedido.Usuario.Email, pedido, pedido.Estatus ?? "Actualizado");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al enviar correo HTML de estatus de orden #{PedidoId} a {Email}", pedido.Id, pedido.Usuario.Email);
                }
            }

            return NoContent();
        }
    }
}

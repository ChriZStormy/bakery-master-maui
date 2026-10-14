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

        private static decimal CalcularMonto(Pedido pedido)
        {
            decimal precioBase = 350m;
            if (pedido.Pastel != null && !string.IsNullOrWhiteSpace(pedido.Pastel.Tamanio))
            {
                precioBase = pedido.Pastel.Tamanio.Trim().ToLower() switch
                {
                    "chico" => 250m,
                    "mediano" => 350m,
                    "grande" => 480m,
                    "extra grande" => 620m,
                    _ => 350m
                };
            }
            int cant = pedido.Cantidad > 0 ? pedido.Cantidad : 1;
            return precioBase * cant;
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido([FromBody] Pedido pedido, [FromServices] PasteleriaAPI.Services.IEmailService emailService)
        {
            pedido.Fecha = DateTime.Now;
            if (string.IsNullOrWhiteSpace(pedido.Estatus))
            {
                pedido.Estatus = "Pendiente";
            }
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Cargar datos completos para generar el comprobante (HU-05 Escenario 1)
            var pedidoCompleto = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Pastel).ThenInclude(p => p.Bizcocho)
                .Include(p => p.Pastel).ThenInclude(p => p.Relleno)
                .Include(p => p.Pastel).ThenInclude(p => p.Glaseado)
                .Include(p => p.Pastel).ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            if (pedidoCompleto?.Usuario != null && !string.IsNullOrWhiteSpace(pedidoCompleto.Usuario.Email))
            {
                var usuario = pedidoCompleto.Usuario;
                var pastel = pedidoCompleto.Pastel;
                var montoTotal = CalcularMonto(pedidoCompleto);
                var fechaEntregaFormateada = pedidoCompleto.FechaEntrega.ToString("dd/MM/yyyy HH:mm");
                var pastelNombre = pastel?.Nombre ?? "Pastel Artesanal";
                var bizcocho = pastel?.Bizcocho?.Nombre ?? "Tradicional";
                var relleno = pastel?.Relleno?.Nombre ?? "Estándar";
                var glaseado = pastel?.Glaseado?.Nombre ?? "Cobertura especial";
                var tamanio = pastel?.Tamanio ?? "Mediano";
                var categoria = pastel?.Categoria?.categoria ?? "General";

                var subject = $"🧾 Comprobante de Pedido #{pedidoCompleto.Id} - Pastelería Dolce Vita";
                
                var body = $"COMPROBANTE DE COMPRA - PASTELERÍA DOLCE VITA\n" +
                           $"================================================\n\n" +
                           $"Estimado(a) {usuario.Nombre},\n\n" +
                           $"¡Gracias por tu compra! Tu orden ha sido registrada exitosamente y se encuentra en nuestro sistema.\n\n" +
                           $"DETALLES DEL PEDIDO:\n" +
                           $"- Folio: #{pedidoCompleto.Id}\n" +
                           $"- Fecha de Solicitud: {pedidoCompleto.Fecha:dd/MM/yyyy HH:mm}\n" +
                           $"- Fecha y Hora de Entrega Requerida: {fechaEntregaFormateada}\n" +
                           $"- Estatus Inicial: {pedidoCompleto.Estatus}\n\n" +
                           $"DETALLES DEL PRODUCTO:\n" +
                           $"- Pastel: {pastelNombre}\n" +
                           $"- Tamaño: {tamanio}\n" +
                           $"- Bizcocho: {bizcocho}\n" +
                           $"- Relleno: {relleno}\n" +
                           $"- Glaseado: {glaseado}\n" +
                           $"- Categoría: {categoria}\n" +
                           $"- Cantidad Solicitada: {pedidoCompleto.Cantidad} unidad(es)\n\n" +
                           $"RESUMEN DE PAGO:\n" +
                           $"- Monto Pagado Total: ${montoTotal:N2} MXN\n" +
                           $"- Estado del Pago: Confirmado / En Firme\n\n" +
                           $"Te estaremos notificando por este medio cuando nuestro taller de repostería inicie la preparación y cuando tu pastel se encuentre listo para entrega.\n\n" +
                           $"Atentamente,\n" +
                           $"Equipo de Pastelería Dolce Vita";

                var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'/>
<style>
  body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #F7F4EF; margin: 0; padding: 20px; }}
  .card {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; box-shadow: 0 4px 15px rgba(139,94,52,0.1); overflow: hidden; border: 1px solid #EAE4DB; }}
  .header {{ background: linear-gradient(135deg, #8B5E34, #5D4037); color: #ffffff; padding: 25px; text-align: center; }}
  .header h1 {{ margin: 0; font-size: 24px; letter-spacing: 1px; }}
  .header p {{ margin: 5px 0 0; opacity: 0.9; font-size: 14px; }}
  .content {{ padding: 25px; color: #3E2723; }}
  .badge {{ display: inline-block; background-color: #FFF3E0; color: #E65100; font-weight: bold; padding: 6px 14px; border-radius: 20px; font-size: 14px; border: 1px solid #FFE0B2; }}
  .section-title {{ font-size: 16px; font-weight: bold; color: #8B5E34; border-bottom: 2px solid #F5F0E6; padding-bottom: 6px; margin-top: 20px; margin-bottom: 12px; }}
  .table {{ width: 100%; border-collapse: collapse; margin-bottom: 15px; }}
  .table td {{ padding: 8px 4px; font-size: 14px; border-bottom: 1px solid #F7F4EF; }}
  .table td.label {{ color: #795548; font-weight: 600; width: 45%; }}
  .total-box {{ background-color: #FAF8F5; border: 2px dashed #8B5E34; border-radius: 12px; padding: 15px; text-align: center; margin: 20px 0; }}
  .total-amount {{ font-size: 26px; font-weight: bold; color: #2E7D32; }}
  .footer {{ background: #FAF8F5; padding: 15px; text-align: center; font-size: 12px; color: #8D6E63; border-top: 1px solid #EAE4DB; }}
</style>
</head>
<body>
  <div class='card'>
    <div class='header'>
      <h1>🍰 Pastelería Dolce Vita</h1>
      <p>Comprobante Oficial de Pedido</p>
    </div>
    <div class='content'>
      <p>Hola <strong>{usuario.Nombre}</strong>,</p>
      <p>¡Gracias por tu compra! Tu orden ha sido registrada exitosamente y se encuentra en nuestro sistema.</p>
      
      <div style='text-align: center; margin: 15px 0;'>
        <span class='badge'>Folio del Pedido: #{pedidoCompleto.Id}</span>
      </div>

      <div class='section-title'>📅 Información de Entrega</div>
      <table class='table'>
        <tr><td class='label'>Fecha de Solicitud:</td><td>{pedidoCompleto.Fecha:dd/MM/yyyy HH:mm}</td></tr>
        <tr><td class='label'>Fecha y Hora Requerida:</td><td><strong>{fechaEntregaFormateada}</strong></td></tr>
        <tr><td class='label'>Estatus Actual:</td><td><span style='color: #D84315; font-weight: bold;'>{pedidoCompleto.Estatus}</span></td></tr>
      </table>

      <div class='section-title'>🎂 Detalle del Pastel</div>
      <table class='table'>
        <tr><td class='label'>Pastel:</td><td><strong>{pastelNombre}</strong></td></tr>
        <tr><td class='label'>Tamaño:</td><td>{tamanio}</td></tr>
        <tr><td class='label'>Bizcocho:</td><td>{bizcocho}</td></tr>
        <tr><td class='label'>Relleno:</td><td>{relleno}</td></tr>
        <tr><td class='label'>Glaseado:</td><td>{glaseado}</td></tr>
        <tr><td class='label'>Categoría:</td><td>{categoria}</td></tr>
        <tr><td class='label'>Cantidad:</td><td><strong>{pedidoCompleto.Cantidad} pieza(s)</strong></td></tr>
      </table>

      <div class='total-box'>
        <div style='font-size: 13px; color: #5D4037; text-transform: uppercase;'>Monto Total Pagado</div>
        <div class='total-amount'>${montoTotal:N2} MXN</div>
        <div style='font-size: 12px; color: #795548;'>Estado: Pago Confirmado / En Firme</div>
      </div>

      <p style='font-size: 13px; color: #5D4037; line-height: 1.5;'>
        Recibirás notificaciones automáticas por correo cuando nuestro taller de repostería inicie la preparación y cuando tu pedido esté listo para entrega.
      </p>
    </div>
    <div class='footer'>
      Pastelería Dolce Vita • Hecho con dedicación artesanal • Soporte: contacto@pasteleria.com
    </div>
  </div>
</body>
</html>";

                await emailService.SendEmailAsync(usuario.Email, subject, body, htmlBody);
            }

            return Ok(pedidoCompleto ?? pedido);
        }

        public class PedidoUpdateDto
        {
            public string Estatus { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedido(int id, [FromBody] PedidoUpdateDto pedidoActualizado, [FromServices] PasteleriaAPI.Services.IEmailService emailService)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Pastel).ThenInclude(p => p.Bizcocho)
                .Include(p => p.Pastel).ThenInclude(p => p.Relleno)
                .Include(p => p.Pastel).ThenInclude(p => p.Glaseado)
                .Include(p => p.Pastel).ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (pedido == null) return NotFound();

            var estatusAnterior = pedido.Estatus;
            pedido.Estatus = pedidoActualizado.Estatus;
            await _context.SaveChangesAsync();

            // Notificación por correo electrónico (HU-05 Escenario 2 y cambios de fase de producción HU-07)
            if (pedido.Usuario != null && !string.IsNullOrWhiteSpace(pedido.Usuario.Email))
            {
                var usuario = pedido.Usuario;
                var pastelNombre = pedido.Pastel?.Nombre ?? "Pastel Artesanal";
                var fechaEntregaStr = pedido.FechaEntrega.ToString("dd/MM/yyyy HH:mm");
                string subject;
                string body;
                string htmlBody;

                if (string.Equals(pedido.Estatus, "Listo para entrega", StringComparison.OrdinalIgnoreCase))
                {
                    // HU-05 Escenario 2: Notificación de pedido terminado
                    subject = $"🎂 ¡Tu pedido #{pedido.Id} está Listo para Entrega! - Pastelería Dolce Vita";
                    
                    body = $"AVISO DE PEDIDO TERMINADO - PASTELERÍA DOLCE VITA\n" +
                           $"================================================\n\n" +
                           $"Estimado(a) {usuario.Nombre},\n\n" +
                           $"¡Tenemos excelentes noticias! Nuestro taller de repostería ha finalizado con éxito el montaje y decoración de tu pedido #{pedido.Id} ({pastelNombre}).\n\n" +
                           $"ESTATUS ACTUAL: {pedido.Estatus}\n\n" +
                           $"AVISO IMPORTANTE:\n" +
                           $"Tu producto ya se encuentra disponible para su recolección en el mostrador de nuestra sucursal o va en ruta hacia tu domicilio según la modalidad acordada.\n\n" +
                           $"Fecha y Hora programada de entrega: {fechaEntregaStr}\n" +
                           $"Cantidad: {pedido.Cantidad} pieza(s)\n\n" +
                           $"¡Esperamos que lo disfrutes muchísimo!\n\n" +
                           $"Atentamente,\n" +
                           $"Equipo de Pastelería Dolce Vita";

                    htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'/>
<style>
  body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #F7F4EF; margin: 0; padding: 20px; }}
  .card {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; box-shadow: 0 4px 15px rgba(46,125,50,0.15); overflow: hidden; border: 1px solid #C8E6C9; }}
  .header {{ background: linear-gradient(135deg, #2E7D32, #1B5E20); color: #ffffff; padding: 25px; text-align: center; }}
  .header h1 {{ margin: 0; font-size: 24px; }}
  .content {{ padding: 25px; color: #1B5E20; }}
  .highlight-box {{ background: #E8F5E9; border-left: 5px solid #2E7D32; padding: 15px; border-radius: 8px; margin: 20px 0; }}
  .badge {{ display: inline-block; background-color: #C8E6C9; color: #1B5E20; font-weight: bold; padding: 6px 14px; border-radius: 20px; font-size: 14px; }}
  .footer {{ background: #FAF8F5; padding: 15px; text-align: center; font-size: 12px; color: #8D6E63; border-top: 1px solid #EAE4DB; }}
</style>
</head>
<body>
  <div class='card'>
    <div class='header'>
      <h1>🎂 ¡Tu Pastel está Listo!</h1>
      <p style='margin: 5px 0 0; opacity: 0.9;'>Fase de Producción Finalizada</p>
    </div>
    <div class='content'>
      <p>Hola <strong>{usuario.Nombre}</strong>,</p>
      <p>¡Tenemos excelentes noticias! Nuestro taller de repostería ha completado la elaboración, montaje y decoración de tu pastel.</p>
      
      <div style='text-align: center; margin: 15px 0;'>
        <span class='badge'>Pedido #{pedido.Id} • Estatus: {pedido.Estatus}</span>
      </div>

      <div class='highlight-box'>
        <strong style='font-size: 16px; color: #1B5E20;'>🚚 Disponible para recolección o en ruta</strong>
        <p style='margin: 8px 0 0; color: #2E7D32; font-size: 14px;'>
          Tu producto ya está listo para entrega. Puedes pasar a recogerlo al mostrador de la sucursal o estará llegando en ruta según la opción seleccionada.
        </p>
      </div>

      <table style='width: 100%; border-collapse: collapse; margin: 15px 0; font-size: 14px; color: #3E2723;'>
        <tr><td style='padding: 6px 0; font-weight: 600; color: #795548;'>Pastel:</td><td><strong>{pastelNombre}</strong></td></tr>
        <tr><td style='padding: 6px 0; font-weight: 600; color: #795548;'>Cantidad:</td><td>{pedido.Cantidad} pieza(s)</td></tr>
        <tr><td style='padding: 6px 0; font-weight: 600; color: #795548;'>Fecha y Hora Programada:</td><td><strong>{fechaEntregaStr}</strong></td></tr>
      </table>

      <p style='font-size: 13px; color: #5D4037;'>
        ¡Gracias por elegirnos para endulzar tu momento especial!
      </p>
    </div>
    <div class='footer'>
      Pastelería Dolce Vita • Taller de Repostería Artesanal
    </div>
  </div>
</body>
</html>";
                }
                else if (string.Equals(pedido.Estatus, "En Preparación", StringComparison.OrdinalIgnoreCase) || string.Equals(pedido.Estatus, "En Proceso", StringComparison.OrdinalIgnoreCase))
                {
                    subject = $"👨‍🍳 Tu pedido #{pedido.Id} está En Preparación - Pastelería Dolce Vita";
                    body = $"Hola {usuario.Nombre},\n\nTe informamos que tu pedido con el Folio #{pedido.Id} ({pastelNombre}) acaba de entrar a nuestro taller de repostería y se encuentra actualmente 'En Preparación'.\n\nNuestro maestro repostero está trabajando en el horneado y elaboración de tu pastel.\n\nFecha/Hora estimada de entrega: {fechaEntregaStr}\n\nTe avisaremos de inmediato cuando esté listo para entrega.\n\nSaludos,\nPastelería Dolce Vita";
                    
                    htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'/>
<style>
  body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #F7F4EF; margin: 0; padding: 20px; }}
  .card {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; box-shadow: 0 4px 15px rgba(25,118,210,0.15); overflow: hidden; border: 1px solid #BBDEFB; }}
  .header {{ background: linear-gradient(135deg, #1976D2, #0D47A1); color: #ffffff; padding: 25px; text-align: center; }}
  .header h1 {{ margin: 0; font-size: 24px; }}
  .content {{ padding: 25px; color: #0D47A1; }}
  .badge {{ display: inline-block; background-color: #E3F2FD; color: #0D47A1; font-weight: bold; padding: 6px 14px; border-radius: 20px; font-size: 14px; }}
  .footer {{ background: #FAF8F5; padding: 15px; text-align: center; font-size: 12px; color: #8D6E63; border-top: 1px solid #EAE4DB; }}
</style>
</head>
<body>
  <div class='card'>
    <div class='header'>
      <h1>👨‍🍳 ¡Manos a la Obra!</h1>
      <p style='margin: 5px 0 0; opacity: 0.9;'>Tu pedido está en el taller de repostería</p>
    </div>
    <div class='content'>
      <p>Hola <strong>{usuario.Nombre}</strong>,</p>
      <p>Te informamos que tu pedido con el Folio <strong>#{pedido.Id}</strong> ({pastelNombre}) ha entrado al taller de repostería y se encuentra en fase <strong>En Preparación</strong>.</p>
      
      <div style='text-align: center; margin: 15px 0;'>
        <span class='badge'>Fase: En Preparación / Horneado y Montaje</span>
      </div>

      <p style='font-size: 14px; color: #3E2723;'>
        Fecha y hora de entrega requerida: <strong>{fechaEntregaStr}</strong>
      </p>
      <p style='font-size: 13px; color: #5D4037;'>
        Te notificaremos en cuanto el repostero finalice el montaje y la decoración para que puedas recogerlo o recibirlo oportunamente.
      </p>
    </div>
    <div class='footer'>
      Pastelería Dolce Vita • Taller de Repostería Artesanal
    </div>
  </div>
</body>
</html>";
                }
                else
                {
                    subject = $"Actualización de Estatus - Pedido #{pedido.Id} - Pastelería Dolce Vita";
                    body = $"Hola {usuario.Nombre},\n\nTu pedido con el Folio #{pedido.Id} ha cambiado su estatus a: {pedido.Estatus}.\n\nSaludos,\nPastelería Dolce Vita";
                    
                    htmlBody = $@"
<!DOCTYPE html>
<html>
<body style='font-family: sans-serif; background: #F7F4EF; padding: 20px;'>
  <div style='max-width: 500px; margin: 0 auto; background: white; padding: 25px; border-radius: 12px; border: 1px solid #EAE4DB;'>
    <h2 style='color: #8B5E34; margin-top: 0;'>Actualización de Pedido #{pedido.Id}</h2>
    <p>Hola <strong>{usuario.Nombre}</strong>,</p>
    <p>Tu pedido con el Folio <strong>#{pedido.Id}</strong> ha cambiado su estatus a:</p>
    <div style='padding: 10px; background: #FAF8F5; border-radius: 8px; font-size: 18px; font-weight: bold; color: #D84315; text-align: center;'>
      {pedido.Estatus}
    </div>
    <p style='font-size: 13px; color: #795548; margin-top: 20px;'>Gracias por tu preferencia,<br>Pastelería Dolce Vita</p>
  </div>
</body>
</html>";
                }

                await emailService.SendEmailAsync(usuario.Email, subject, body, htmlBody);
            }

            return NoContent();
        }
    }
}

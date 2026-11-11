using System.Globalization;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
        Task EnviarComprobantePedidoAsync(string toEmail, Pedido pedido);
        Task EnviarActualizacionEstatusAsync(string toEmail, Pedido pedido, string nuevoEstatus);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                message.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = body
                };

                using var client = new SmtpClient();

                // Conexión segura estándar STARTTLS (Mailtrap Sandbox puerto 2525 o 587)
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);

                // Autenticación con credenciales provistas por la configuración
                var authUser = !string.IsNullOrWhiteSpace(_smtpSettings.Username) 
                    ? _smtpSettings.Username 
                    : _smtpSettings.SenderEmail;

                await client.AuthenticateAsync(authUser, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Correo enviado con éxito a {ToEmail} a través de {Server}:{Port} [Asunto: {Subject}]",
                    toEmail, _smtpSettings.Server, _smtpSettings.Port, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al enviar correo a {ToEmail} a través de {Server}:{Port}. Detalle: {Message}",
                    toEmail, _smtpSettings.Server, _smtpSettings.Port, ex.Message);
                throw;
            }
        }

        public async Task EnviarComprobantePedidoAsync(string toEmail, Pedido pedido)
        {
            var nombrePasteleria = !string.IsNullOrWhiteSpace(_smtpSettings.SenderName) 
                ? _smtpSettings.SenderName 
                : "Dolce Vita Pastelería";

            var folio = pedido.Id > 0 ? $"#PED-{pedido.Id:D5}" : $"#PED-{pedido.Id}";
            var fechaEntrega = pedido.FechaEntrega.ToString("dddd, dd 'de' MMMM 'de' yyyy", CultureInfo.GetCultureInfo("es-MX"));
            var horaEntrega = !string.IsNullOrEmpty(pedido.HoraEntrega) ? pedido.HoraEntrega : "Horario regular";
            var modalidad = !string.IsNullOrEmpty(pedido.ModalidadEntrega) ? pedido.ModalidadEntrega : "Recolección en Tienda";
            var direccion = !string.IsNullOrEmpty(pedido.Direccion) ? pedido.Direccion : "Recolección en sucursal principal";
            var totalFormateado = pedido.Total.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

            var nombrePastel = pedido.Pastel?.Nombre ?? "Pastel Artesanal";
            var tamanio = pedido.Pastel?.Tamanio ?? "Mediano";

            // Capas opcionales si es personalizado
            var detallesCapas = string.Empty;
            if (pedido.Pastel?.Bizcocho != null || pedido.Pastel?.Relleno != null || pedido.Pastel?.Glaseado != null)
            {
                detallesCapas = $@"
                    <tr>
                        <td style='padding: 6px 0; color: #795548; font-size: 13px;'><strong>Capas elegidas:</strong></td>
                        <td style='padding: 6px 0; color: #3E2723; font-size: 13px; text-align: right;'>
                            Pan: {pedido.Pastel?.Bizcocho?.Nombre ?? "Especial"} | 
                            Relleno: {pedido.Pastel?.Relleno?.Nombre ?? "Especial"} | 
                            Cubierta: {pedido.Pastel?.Glaseado?.Nombre ?? "Especial"}
                        </td>
                    </tr>";
            }

            var seccionMensaje = string.Empty;
            if (!string.IsNullOrWhiteSpace(pedido.Mensaje))
            {
                seccionMensaje = $@"
                    <div style='background-color: #FAF8F5; border-left: 4px solid #8B5E34; padding: 12px; margin: 15px 0; border-radius: 4px;'>
                        <span style='font-size: 11px; text-transform: uppercase; color: #8D6E63; font-weight: bold;'>Dedicatoria en el pastel:</span>
                        <p style='margin: 4px 0 0 0; font-size: 14px; font-style: italic; color: #3E2723;'>&ldquo;{pedido.Mensaje}&rdquo;</p>
                    </div>";
            }

            var html = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Comprobante de Orden - {nombrePasteleria}</title>
</head>
<body style='margin: 0; padding: 0; background-color: #F7F4EF; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif;'>
    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;'>
        <tr>
            <td align='center' style='padding: 30px 15px;'>
                <!-- Card Principal -->
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; background-color: #FFFFFF; border-radius: 16px; border: 1px solid #EAE4DB; box-shadow: 0 4px 15px rgba(139, 94, 52, 0.08); overflow: hidden;'>
                    <!-- Header -->
                    <tr>
                        <td align='center' style='background: linear-gradient(135deg, #8B5E34 0%, #5D4037 100%); padding: 30px 20px; color: #FFFFFF;'>
                            <h1 style='margin: 0; font-size: 24px; font-weight: bold; letter-spacing: 0.5px;'>🎂 {nombrePasteleria}</h1>
                            <p style='margin: 8px 0 0 0; font-size: 14px; opacity: 0.9;'>¡Comprobante de Orden Confirmada!</p>
                        </td>
                    </tr>

                    <!-- Cuerpo -->
                    <tr>
                        <td style='padding: 30px 25px;'>
                            <p style='margin: 0 0 20px 0; font-size: 15px; color: #5D4037; line-height: 1.5;'>
                                Hola, hemos recibido y programado tu pedido con éxito. A continuación encontrarás el desglose completo de tu orden:
                            </p>

                            <!-- Folio y Estatus -->
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #FAF8F5; border-radius: 10px; border: 1px solid #E0D7CD; margin-bottom: 20px;'>
                                <tr>
                                    <td style='padding: 15px;'>
                                        <span style='font-size: 11px; text-transform: uppercase; color: #8D6E63; font-weight: bold;'>Número de Folio:</span>
                                        <div style='font-size: 20px; font-weight: bold; color: #8B5E34; margin-top: 2px;'>{folio}</div>
                                    </td>
                                    <td align='right' style='padding: 15px;'>
                                        <span style='background-color: #E8F5E9; color: #2E7D32; font-weight: bold; font-size: 12px; padding: 6px 12px; border-radius: 20px; border: 1px solid #C8E6C9;'>
                                            {pedido.Estatus ?? "Pendiente"}
                                        </span>
                                    </td>
                                </tr>
                            </table>

                            <!-- Detalle del Pastel -->
                            <h3 style='margin: 20px 0 10px 0; font-size: 16px; color: #3E2723; border-bottom: 2px solid #EAE4DB; padding-bottom: 6px;'>
                                🍰 Detalle del Producto
                            </h3>
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-bottom: 15px;'>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Pastel:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; font-weight: bold; text-align: right;'>{nombrePastel}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Tamaño:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; text-align: right;'>{tamanio}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Cantidad:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; font-weight: bold; text-align: right;'>{pedido.Cantidad} pieza(s)</td>
                                </tr>
                                {detallesCapas}
                            </table>

                            {seccionMensaje}

                            <!-- Logística de Entrega -->
                            <h3 style='margin: 25px 0 10px 0; font-size: 16px; color: #3E2723; border-bottom: 2px solid #EAE4DB; padding-bottom: 6px;'>
                                📅 Programación y Logística
                            </h3>
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-bottom: 20px;'>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Fecha programada:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; text-align: right;'>{fechaEntrega}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Hora de entrega:</strong></td>
                                    <td style='padding: 6px 0; color: #8B5E34; font-size: 14px; font-weight: bold; text-align: right;'>{horaEntrega}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Modalidad:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; text-align: right;'>{modalidad}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 6px 0; color: #795548; font-size: 14px;'><strong>Dirección / Entrega:</strong></td>
                                    <td style='padding: 6px 0; color: #3E2723; font-size: 14px; text-align: right;'>{direccion}</td>
                                </tr>
                            </table>

                            <!-- Total a pagar -->
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #FAF8F5; border-radius: 10px; border: 1px solid #EAE4DB; margin-top: 10px;'>
                                <tr>
                                    <td style='padding: 16px; color: #3E2723; font-size: 15px; font-weight: bold;'>Total a Pagar:</td>
                                    <td align='right' style='padding: 16px; color: #2E7D32; font-size: 22px; font-weight: bold;'>{totalFormateado}</td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td align='center' style='background-color: #FAF8F5; padding: 20px; border-top: 1px solid #EAE4DB; font-size: 12px; color: #8D6E63;'>
                            <p style='margin: 0;'>Gracias por preferir a <strong>{nombrePasteleria}</strong>.</p>
                            <p style='margin: 4px 0 0 0;'>Elaboramos cada pastel con pasión y los más selectos ingredientes.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            await SendEmailAsync(toEmail, $"Comprobante de Orden {folio} - {nombrePasteleria}", html, isHtml: true);
        }

        public async Task EnviarActualizacionEstatusAsync(string toEmail, Pedido pedido, string nuevoEstatus)
        {
            var nombrePasteleria = !string.IsNullOrWhiteSpace(_smtpSettings.SenderName) 
                ? _smtpSettings.SenderName 
                : "Dolce Vita Pastelería";

            var folio = pedido.Id > 0 ? $"#PED-{pedido.Id:D5}" : $"#PED-{pedido.Id}";
            var clienteNombre = !string.IsNullOrWhiteSpace(pedido.Usuario?.Nombre) 
                ? pedido.Usuario.Nombre 
                : "Estimado/a Cliente";
            var nombrePastel = pedido.Pastel?.Nombre ?? "Pastel Artesanal";
            var fechaEntrega = pedido.FechaEntrega.ToString("dddd, dd 'de' MMMM 'de' yyyy", CultureInfo.GetCultureInfo("es-MX"));
            var horaEntrega = !string.IsNullOrEmpty(pedido.HoraEntrega) ? pedido.HoraEntrega : "Horario regular";

            var (badgeBg, badgeColor, icono, descripcionFase) = nuevoEstatus switch
            {
                "Horneado" => ("#FBE9E7", "#BF360C", "🔥", "¡Tu pastel ha entrado a la fase de horneado! La base está tomando forma con la mejor temperatura y esponjosidad."),
                "Decoración" => ("#F3E5F5", "#6A1B9A", "🎨", "¡Nuestros maestros reposteros están decorando tu pastel con los mejores ingredientes y detalles artesanales!"),
                "Listo" => ("#E8F5E9", "#2E7D32", "✅", "¡Tu pastel está terminado y listo! Puedes pasar a recogerlo o estará en camino según la modalidad elegida."),
                "Entregado" => ("#ECEFF1", "#37474F", "📦", "Tu pedido ha sido entregado exitosamente. ¡Esperamos que disfrutes cada bocado!"),
                _ => ("#FFF3E0", "#E65100", "⏳", $"Tu pedido ha cambiado de estado a: {nuevoEstatus}")
            };

            var html = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Actualización de Pedido - {nombrePasteleria}</title>
</head>
<body style='margin: 0; padding: 0; background-color: #F7F4EF; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif;'>
    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;'>
        <tr>
            <td align='center' style='padding: 30px 15px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 580px; background-color: #FFFFFF; border-radius: 16px; border: 1px solid #EAE4DB; box-shadow: 0 4px 15px rgba(139, 94, 52, 0.08); overflow: hidden;'>
                    <!-- Header -->
                    <tr>
                        <td align='center' style='background: linear-gradient(135deg, #8B5E34 0%, #5D4037 100%); padding: 25px 20px; color: #FFFFFF;'>
                            <h1 style='margin: 0; font-size: 22px; font-weight: bold;'>🎂 {nombrePasteleria}</h1>
                            <p style='margin: 6px 0 0 0; font-size: 14px; opacity: 0.9;'>Notificación de Avance de Producción</p>
                        </td>
                    </tr>

                    <!-- Contenido -->
                    <tr>
                        <td style='padding: 25px 20px;'>
                            <p style='margin: 0 0 15px 0; font-size: 15px; color: #3E2723;'>
                                Hola <strong>{clienteNombre}</strong>,
                            </p>
                            <p style='margin: 0 0 20px 0; font-size: 14px; color: #5D4037; line-height: 1.5;'>
                                Te informamos que tu orden <strong>{folio}</strong> ha avanzado en nuestra cocina:
                            </p>

                            <!-- Badge de Fase Actual -->
                            <div style='background-color: {badgeBg}; border: 1px solid {badgeColor}; border-radius: 12px; padding: 16px; text-align: center; margin-bottom: 20px;'>
                                <span style='font-size: 28px;'>{icono}</span>
                                <h2 style='margin: 8px 0 4px 0; color: {badgeColor}; font-size: 18px;'>{nuevoEstatus}</h2>
                                <p style='margin: 0; font-size: 13px; color: #4E342E;'>{descripcionFase}</p>
                            </div>

                            <!-- Recordatorio del Pedido -->
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: #FAF8F5; border-radius: 10px; border: 1px solid #EAE4DB; padding: 12px; margin-bottom: 15px;'>
                                <tr>
                                    <td style='padding: 4px 8px; color: #795548; font-size: 13px;'><strong>Producto:</strong></td>
                                    <td style='padding: 4px 8px; color: #3E2723; font-size: 13px; text-align: right;'>{nombrePastel}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; color: #795548; font-size: 13px;'><strong>Entrega Programada:</strong></td>
                                    <td style='padding: 4px 8px; color: #8B5E34; font-size: 13px; font-weight: bold; text-align: right;'>{fechaEntrega} a las {horaEntrega}</td>
                                </tr>
                            </table>

                            <p style='margin: 15px 0 0 0; font-size: 13px; color: #8D6E63; text-align: center;'>
                                Puedes consultar el avance en vivo en cualquier momento desde la aplicación en tu sección de <em>Mis Pedidos</em>.
                            </p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td align='center' style='background-color: #FAF8F5; padding: 15px; border-top: 1px solid #EAE4DB; font-size: 12px; color: #8D6E63;'>
                            <p style='margin: 0;'>{nombrePasteleria} &bull; Pasión artesanal en cada detalle.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            await SendEmailAsync(toEmail, $"Actualización de Orden {folio}: {icono} {nuevoEstatus} - {nombrePasteleria}", html, isHtml: true);
        }
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace PasteleriaAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string? htmlBody = null);
    }

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body, string? htmlBody = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Pasteleria App", "no-reply@pasteleria.com"));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder();
                builder.TextBody = body;
                if (!string.IsNullOrWhiteSpace(htmlBody))
                {
                    builder.HtmlBody = htmlBody;
                }
                message.Body = builder.ToMessageBody();

                // 1. SIMULACIÓN: Guardar el correo en una carpeta local para verificar que funciona
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CorreosEnviados");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var cleanEmail = toEmail.Replace("@", "_at_").Replace(".", "_");
                
                var txtFilePath = Path.Combine(folderPath, $"Correo_{timestamp}_{cleanEmail}.txt");
                await File.WriteAllTextAsync(txtFilePath, $"PARA: {toEmail}\nASUNTO: {subject}\n\nCUERPO:\n{body}");
                Console.WriteLine($"[EXITO] Correo simulado guardado en: {txtFilePath}");

                if (!string.IsNullOrWhiteSpace(htmlBody))
                {
                    var htmlFilePath = Path.Combine(folderPath, $"Correo_{timestamp}_{cleanEmail}.html");
                    await File.WriteAllTextAsync(htmlFilePath, htmlBody);
                    Console.WriteLine($"[EXITO] Vista previa HTML guardada en: {htmlFilePath}");
                }

                // 2. PRODUCCIÓN: Envío Real (Requiere credenciales reales)
                /*
                using var client = new SmtpClient();
                // Servidor SMTP de Outlook/Hotmail
                await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                
                // Reemplaza con tu correo y tu "Contraseña de Aplicación" generada en Microsoft
                await client.AuthenticateAsync("pasteleriamoviles@outlook.com", "tu_contraseña_de_aplicacion");
                
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}

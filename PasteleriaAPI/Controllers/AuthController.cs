using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]

        // Se recibe el payload encriptado desde el frontend por seguridad. 
        // Luego se desencripta para validar las credenciales contra la base de datos.  
        public async Task<ActionResult<Usuario>> Login([FromBody] PasteleriaAPI.Helpers.EncryptedPayload payload)
        {
            try 
            {
                var json = PasteleriaAPI.Helpers.CryptoHelper.Decrypt(payload.Data);
                var credenciales = System.Text.Json.JsonSerializer.Deserialize<Usuario>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == credenciales.Email && u.Password == credenciales.Password);
                if (user == null) return Unauthorized("Credenciales incorrectas");
                return Ok(user);
            }
            catch { return BadRequest("Invalid Payload"); }
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] PasteleriaAPI.Helpers.EncryptedPayload payload)
        {
            try
            {
                var json = PasteleriaAPI.Helpers.CryptoHelper.Decrypt(payload.Data);
                var nuevoUsuario = System.Text.Json.JsonSerializer.Deserialize<Usuario>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Validaciones por expresiones regulares (Regex) para asegurar que 
                // el formato del correo sea válido y la contraseña sea lo suficientemente segura.
                
                // Backend Regex Validations
                var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                var passRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                
                if (!emailRegex.IsMatch(nuevoUsuario.Email)) return BadRequest("Formato de correo inválido");
                if (!passRegex.IsMatch(nuevoUsuario.Password)) return BadRequest("La contraseña no cumple con los requisitos de seguridad");

                if (await _context.Usuarios.AnyAsync(u => u.Email == nuevoUsuario.Email && u.Rol == nuevoUsuario.Rol))
                {
                    return BadRequest($"Ya existe una cuenta con el rol de {nuevoUsuario.Rol} para este correo electrónico");
                }

                if (string.IsNullOrEmpty(nuevoUsuario.Rol)) nuevoUsuario.Rol = "Cliente";

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                return Ok(nuevoUsuario);
            }
            catch { return BadRequest("Invalid Payload"); }
        }
    }
}

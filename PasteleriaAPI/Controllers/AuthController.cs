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
        public async Task<ActionResult<Usuario>> Login([FromBody] Usuario credenciales)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == credenciales.Email && u.Password == credenciales.Password);
            if (user == null) return Unauthorized("Credenciales incorrectas");
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] Usuario nuevoUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == nuevoUsuario.Email))
            {
                return BadRequest("El correo electrónico ya está en uso");
            }

            // Asignar rol de cliente por defecto si no se especifica
            if (string.IsNullOrEmpty(nuevoUsuario.Rol)) nuevoUsuario.Rol = "Cliente";

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();
            return Ok(nuevoUsuario);
        }
    }
}

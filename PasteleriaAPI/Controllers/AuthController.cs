using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PasteleriaAPI.DTOs;
using PasteleriaAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Credenciales incorrectas");

            if (!user.Activo)
                return Unauthorized("Esta cuenta ha sido dada de baja o suspendida temporalmente.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
                return Unauthorized("Credenciales incorrectas");

            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Nombre = user.Nombre ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Rol = roles.Contains("Admin") ? "Admin" :
                          roles.Contains("Repostero") ? "Repostero" :
                          roles.Contains("Empleado") ? "Repostero" :
                          (roles.FirstOrDefault() ?? "Cliente")
                }
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("Ya existe una cuenta con este correo electrónico.");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nombre = dto.Nombre
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

            // SECURITY: Always assign 'Cliente' role. Admin promotion only via AdminController.
            await _userManager.AddToRoleAsync(user, "Cliente");

            var token = await GenerateJwtToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Nombre = user.Nombre ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Rol = "Cliente"
                }
            });
        }

#if DEBUG
        [HttpPost("debug-switch/{targetRole}")]
        public async Task<ActionResult<AuthResponseDto>> DebugSwitchRole(string targetRole, [FromServices] IWebHostEnvironment env)
        {
            // Seguridad: Solo permitido en ambiente de desarrollo
            if (!env.IsDevelopment())
            {
                return NotFound();
            }

            if (targetRole != "Admin" && targetRole != "Cliente" && targetRole != "Repostero")
            {
                return BadRequest("El rol solicitado debe ser 'Admin', 'Repostero' o 'Cliente'.");
            }

            var currentUserId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser? user = null;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                user = await _userManager.FindByIdAsync(currentUserId);
            }

            if (user == null)
            {
                user = _userManager.Users.FirstOrDefault();
                if (user == null)
                {
                    return NotFound("No se encontró ningún usuario.");
                }
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, targetRole == "Admin" ? "Admin Master (Debug)" : "Cliente Juan (Debug)"),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, targetRole)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthResponseDto
            {
                Token = tokenString,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Nombre = targetRole == "Admin" ? "Admin Master (Debug)" : "Cliente Juan (Debug)",
                    Email = user.Email ?? string.Empty,
                    Rol = targetRole
                }
            });
        }
#endif

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.Nombre ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

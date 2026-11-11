using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasteleriaAPI.DTOs;
using PasteleriaAPI.Entities;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "LocalNetworkOnly")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPut("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Role))
                return BadRequest("Datos incompletos. Se requiere UserId y Role.");

            var validRoles = new[] { "Admin", "Repostero", "Cliente", "Empleado" };
            var targetRole = validRoles.FirstOrDefault(r => r.Equals(dto.Role.Trim(), StringComparison.OrdinalIgnoreCase));
            if (targetRole == null)
                return BadRequest("Rol inválido. Los roles permitidos son: Admin, Repostero, Cliente.");

            if (targetRole.Equals("Empleado", StringComparison.OrdinalIgnoreCase))
                targetRole = "Repostero";

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            var currentAdminId = User.FindFirst("id")?.Value;
            if (dto.UserId == currentAdminId && !targetRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("No puedes removerte a ti mismo del rol de Administrador.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains(targetRole))
            {
                return BadRequest($"El usuario '{user.Nombre}' ya cuenta con el rol '{targetRole}'.");
            }

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return BadRequest(string.Join(", ", removeResult.Errors.Select(e => e.Description)));
            }

            var addResult = await _userManager.AddToRoleAsync(user, targetRole);
            if (!addResult.Succeeded)
                return BadRequest(string.Join(", ", addResult.Errors.Select(e => e.Description)));

            _logger.LogInformation(
                "Rol de usuario {UserId} ({Email}) cambiado a {NewRole} por {AdminId}",
                dto.UserId, user.Email, targetRole, currentAdminId);

            return Ok(new { Message = $"Rol de '{user.Nombre}' actualizado a '{targetRole}' exitosamente." });
        }

        [HttpPut("users/{userId}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var currentAdminId = User.FindFirst("id")?.Value;
            if (userId == currentAdminId)
                return BadRequest("No puedes dar de baja o suspender tu propia cuenta de Administrador.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            // Baja lógica / Reactivación
            user.Activo = !user.Activo;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

            _logger.LogInformation(
                "Estado de usuario {UserId} ({Email}) modificado a Activo={Activo} por {AdminId}",
                userId, user.Email, user.Activo, currentAdminId);

            var estadoStr = user.Activo ? "reactivado" : "dado de baja temporalmente";
            return Ok(new { Message = $"Usuario '{user.Nombre}' ha sido {estadoStr} exitosamente.", Activo = user.Activo });
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Nombre y Email son obligatorios.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            var existingByEmail = await _userManager.FindByEmailAsync(dto.Email.Trim());
            if (existingByEmail != null && existingByEmail.Id != userId)
                return BadRequest("El correo electrónico ya está en uso por otra cuenta.");

            user.Nombre = dto.Nombre.Trim();
            user.Email = dto.Email.Trim();
            user.UserName = dto.Email.Trim();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

            _logger.LogInformation(
                "Usuario {UserId} modificado exitosamente por {AdminId}",
                userId, User.FindFirst("id")?.Value);

            return Ok(new { Message = $"Datos del usuario '{user.Nombre}' actualizados exitosamente." });
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UsuarioDto>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UsuarioDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var rol = roles.Contains("Admin") ? "Admin" :
                          roles.Contains("Repostero") ? "Repostero" :
                          roles.Contains("Empleado") ? "Repostero" :
                          (roles.FirstOrDefault() ?? "Cliente");

                result.Add(new UsuarioDto
                {
                    Id = user.Id,
                    Nombre = user.Nombre ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Rol = rol,
                    Activo = user.Activo
                });
            }

            return Ok(result);
        }
    }
}

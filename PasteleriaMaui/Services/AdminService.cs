using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public interface IAdminService
    {
        Task<List<Usuario>> GetUsersAsync();
        Task<(bool Success, string Message)> AssignRoleAsync(string userId, string newRole);
        Task<(bool Success, string Message)> ToggleUserStatusAsync(string userId);
        Task<(bool Success, string Message)> UpdateUserAsync(string userId, string nombre, string email);
    }

    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;
#if ANDROID
        private readonly string _baseUrl = "http://10.0.2.2:5105/";
#else
        private readonly string _baseUrl = "http://127.0.0.1:5105/";
#endif

        public AdminService()
        {
            var handler = MauiProgram.GetHttpClientHandler();
            var authHandler = new AuthHttpHandler(handler);
            _httpClient = new HttpClient(authHandler);
        }

        public async Task<List<Usuario>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}api/admin/users");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>();
                }
            }
            catch { }
            return new List<Usuario>();
        }

        public async Task<(bool Success, string Message)> AssignRoleAsync(string userId, string newRole)
        {
            try
            {
                var payload = new { UserId = userId, Role = newRole };
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/admin/assign-role", payload);
                if (response.IsSuccessStatusCode)
                {
                    return (true, $"Rol actualizado a '{newRole}' exitosamente.");
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    errorMsg = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Forbidden => "Acceso denegado: solo permitido desde la red local autorizada.",
                        System.Net.HttpStatusCode.Unauthorized => "Sesión no válida o expirada.",
                        System.Net.HttpStatusCode.NotFound => "Usuario no encontrado.",
                        _ => $"Error en el servidor ({response.StatusCode})."
                    };
                }
                return (false, errorMsg);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión con el servidor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ToggleUserStatusAsync(string userId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_baseUrl}api/admin/users/{userId}/toggle-status", null);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Estado de usuario actualizado exitosamente.");
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                return (false, string.IsNullOrWhiteSpace(errorMsg) ? "Error al actualizar estado del usuario." : errorMsg);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión con el servidor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(string userId, string nombre, string email)
        {
            try
            {
                var payload = new { Nombre = nombre, Email = email };
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/admin/users/{userId}", payload);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Datos de usuario actualizados exitosamente.");
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                return (false, string.IsNullOrWhiteSpace(errorMsg) ? "Error al actualizar datos del usuario." : errorMsg);
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión con el servidor: {ex.Message}");
            }
        }
    }
}

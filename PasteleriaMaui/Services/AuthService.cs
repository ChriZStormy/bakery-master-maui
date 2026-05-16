using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
            ? "http://10.0.2.2:5105/api/auth" 
            : "http://localhost:5105/api/auth";

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            var credenciales = new Usuario { Email = email, Password = password };
            var json = System.Text.Json.JsonSerializer.Serialize(credenciales);
            var payload = new PasteleriaMaui.Helpers.EncryptedPayload { Data = PasteleriaMaui.Helpers.CryptoHelper.Encrypt(json) };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/login", payload);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            return null;
        }

        public async Task<Usuario> RegisterAsync(Usuario nuevoUsuario)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(nuevoUsuario);
            var payload = new PasteleriaMaui.Helpers.EncryptedPayload { Data = PasteleriaMaui.Helpers.CryptoHelper.Encrypt(json) };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/register", payload);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            return null;
        }
    }
}

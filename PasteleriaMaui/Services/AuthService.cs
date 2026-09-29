using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class AuthService
    {
        private readonly HttpClient _httpClient;
#if ANDROID
        private readonly string _baseUrl = "http://10.0.2.2:5105/";
#else
        private readonly string _baseUrl = "http://127.0.0.1:5105/";
#endif

        public AuthService()
        {
            var handler = MauiProgram.GetHttpClientHandler();
            var authHandler = new AuthHttpHandler(handler);
            _httpClient = new HttpClient(authHandler);
        }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            var credenciales = new Usuario { Email = email, Password = password };
            var json = System.Text.Json.JsonSerializer.Serialize(credenciales);
            var payload = new PasteleriaMaui.Helpers.EncryptedPayload { Data = PasteleriaMaui.Helpers.CryptoHelper.Encrypt(json) };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/auth/login", payload);
            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResult != null)
                {
                    await SecureStorage.SetAsync("jwt_token", authResult.Token);
                    AppSession.Token = authResult.Token;
                    return authResult.Usuario;
                }
            }
            return null;
        }

        public async Task<Usuario> RegisterAsync(Usuario nuevoUsuario)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(nuevoUsuario);
            var payload = new PasteleriaMaui.Helpers.EncryptedPayload { Data = PasteleriaMaui.Helpers.CryptoHelper.Encrypt(json) };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/auth/register", payload);
            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResult != null)
                {
                    await SecureStorage.SetAsync("jwt_token", authResult.Token);
                    AppSession.Token = authResult.Token;
                    return authResult.Usuario;
                }
            }
            return null;
        }
    }
}

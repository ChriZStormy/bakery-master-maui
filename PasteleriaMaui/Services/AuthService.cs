using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
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

        public async Task<Usuario?> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequest { Email = email, Password = password };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/auth/login", loginRequest);
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

        public async Task<Usuario?> RegisterAsync(string nombre, string email, string password)
        {
            // SECURITY: No role field sent. Server always assigns 'Cliente'.
            var registerRequest = new RegisterRequest
            {
                Nombre = nombre,
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/auth/register", registerRequest);
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

#if DEBUG
        public async Task<AuthResponse?> DebugSwitchRoleAsync(string targetRole)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}api/auth/debug-switch/{targetRole}", null);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AuthResponse>();
                }
            }
            catch { }
            return null;
        }
#endif
    }
}

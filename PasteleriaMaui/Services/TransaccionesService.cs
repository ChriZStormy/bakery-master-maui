using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class TransaccionesService
    {
        private readonly HttpClient _httpClient;
#if ANDROID
        private readonly string _baseUrl = "http://10.0.2.2:5105/";
#else
        private readonly string _baseUrl = "http://127.0.0.1:5105/";
#endif

        public TransaccionesService()
        {
            var handler = MauiProgram.GetHttpClientHandler();
            var authHandler = new AuthHttpHandler(handler);
            _httpClient = new HttpClient(authHandler);
        }

        public async Task<List<Pedido>> GetPedidosAsync(string? usuarioId = null)
        {
            try
            {
                var url = $"{_baseUrl}api/pedido" + (!string.IsNullOrEmpty(usuarioId) ? $"?usuarioId={usuarioId}" : "");
                return await _httpClient.GetFromJsonAsync<List<Pedido>>(url);
            }
            catch { return new List<Pedido>(); }
        }

        public async Task<List<Merma>> GetMermasAsync(string? usuarioId = null)
        {
            try
            {
                var url = $"{_baseUrl}api/merma" + (!string.IsNullOrEmpty(usuarioId) ? $"?usuarioId={usuarioId}" : "");
                return await _httpClient.GetFromJsonAsync<List<Merma>>(url);
            }
            catch { return new List<Merma>(); }
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
        }

        public async Task<(bool Exito, Pedido? PedidoCreado)> RegistrarPedidoAsync(Pedido pedido)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pedido", pedido, GetJsonOptions());
                if (response.IsSuccessStatusCode)
                {
                    var creado = await response.Content.ReadFromJsonAsync<Pedido>(GetJsonOptions());
                    return (true, creado ?? pedido);
                }
                return (false, null);
            }
            catch
            {
                return (false, null);
            }
        }

        public async Task<bool> CrearPedidoAsync(Pedido pedido)
        {
            var res = await RegistrarPedidoAsync(pedido);
            return res.Exito;
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarPedidoAsync(Pedido pedido)
        {
            try
            {
                var payload = new { Estatus = pedido.Estatus, Notas = pedido.Notas };
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/pedido/{pedido.Id}", payload, GetJsonOptions());
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Estatus actualizado correctamente.");
                }
                
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            catch (Exception ex) 
            { 
                return (false, $"Excepción: {ex.Message}"); 
            }
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarEstatusPedidoAsync(int id, string nuevoEstatus, string? notas = null)
        {
            try
            {
                var payload = new { Estatus = nuevoEstatus, Notas = notas };
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/pedido/{id}", payload, GetJsonOptions());
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Estatus actualizado correctamente.");
                }
                
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            catch (Exception ex) 
            { 
                return (false, $"Excepción: {ex.Message}"); 
            }
        }

        public async Task<bool> CrearMermaAsync(Merma merma)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/merma", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarMermaAsync(Merma merma)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/merma/{merma.Id}", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}

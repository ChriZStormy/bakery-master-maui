using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class TransaccionesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
            ? "http://10.0.2.2:5105/api" 
            : "http://localhost:5105/api";

        public TransaccionesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Pedido>> GetPedidosAsync(int? usuarioId)
        {
            try
            {
                var url = $"{_baseUrl}/pedido" + (usuarioId.HasValue ? $"?usuarioId={usuarioId}" : "");
                return await _httpClient.GetFromJsonAsync<List<Pedido>>(url);
            }
            catch { return new List<Pedido>(); }
        }

        public async Task<List<Merma>> GetMermasAsync(int? usuarioId)
        {
            try
            {
                var url = $"{_baseUrl}/merma" + (usuarioId.HasValue ? $"?usuarioId={usuarioId}" : "");
                return await _httpClient.GetFromJsonAsync<List<Merma>>(url);
            }
            catch { return new List<Merma>(); }
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
        }

        public async Task<bool> CrearPedidoAsync(Pedido pedido)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/pedido", pedido, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarPedidoAsync(Pedido pedido)
        {
            try
            {
                var payload = new { Estatus = pedido.Estatus };
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/pedido/{pedido.Id}", payload, GetJsonOptions());
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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/merma", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarMermaAsync(Merma merma)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/merma/{merma.Id}", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}

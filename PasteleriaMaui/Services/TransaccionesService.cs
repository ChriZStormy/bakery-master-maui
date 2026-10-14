using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class TransaccionesService
    {
        private readonly HttpClient _httpClient;
        private string BaseUrl => PasteleriaMaui.Helpers.ApiConfig.BaseUrl;

        public TransaccionesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Pedido>> GetPedidosAsync(int? usuarioId)
        {
            try
            {
                var url = $"{BaseUrl}api/pedido" + (usuarioId.HasValue ? $"?usuarioId={usuarioId}" : "");
                return await _httpClient.GetFromJsonAsync<List<Pedido>>(url);
            }
            catch { return new List<Pedido>(); }
        }

        public async Task<List<Merma>> GetMermasAsync(int? usuarioId)
        {
            try
            {
                var url = $"{BaseUrl}api/merma" + (usuarioId.HasValue ? $"?usuarioId={usuarioId}" : "");
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
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}api/pedido", pedido, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarEstatusPedidoAsync(int pedidoId, string nuevoEstatus)
        {
            try
            {
                var payload = new { Estatus = nuevoEstatus };
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}api/pedido/{pedidoId}", payload, GetJsonOptions());
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

        public async Task<(bool Exito, string Mensaje)> ActualizarPedidoAsync(Pedido pedido)
        {
            return await ActualizarEstatusPedidoAsync(pedido.Id, pedido.Estatus);
        }

        public async Task<bool> CrearMermaAsync(Merma merma)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}api/merma", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarMermaAsync(Merma merma)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}api/merma/{merma.Id}", merma, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}

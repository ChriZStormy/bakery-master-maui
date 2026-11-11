using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
	public class PastelApiService
	{
		private readonly HttpClient _httpClient;

#if ANDROID
        private readonly string _baseUrl = "http://10.0.2.2:5105/";
#else
        private readonly string _baseUrl = "http://127.0.0.1:5105/";
#endif

        public string? UltimoError { get; private set; }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true
            };
        }

		public PastelApiService()
		{
			var handler = MauiProgram.GetHttpClientHandler();
			var authHandler = new AuthHttpHandler(handler);
			_httpClient = new HttpClient(authHandler);
		}

		public async Task<Pastel?> RegistrarPastelAsync(Pastel pastel)
		{
            UltimoError = null;
			try
			{
				var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevopastel", pastel, GetJsonOptions());
				if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Pastel>(GetJsonOptions());
                }
                var errorBody = await response.Content.ReadAsStringAsync();
                UltimoError = $"HTTP {(int)response.StatusCode} ({response.StatusCode}): {errorBody}";
                Console.WriteLine($"Error al registrar pastel: {UltimoError}");
                return null;
			}
			catch (Exception ex)
			{
                UltimoError = $"Excepción: {ex.Message}";
				Console.WriteLine($"Error al conectar con la API: {ex.Message}");
				return null;
			}
		}

        public async Task<List<CatBizcocho>> GetBizcochosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatBizcocho>>($"{_baseUrl}api/pastel/bizcochos", GetJsonOptions()) ?? new List<CatBizcocho>(); }  catch { return new List<CatBizcocho>(); }
        }

        public async Task<List<CatRelleno>> GetRellenosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatRelleno>>($"{_baseUrl}api/pastel/rellenos", GetJsonOptions()) ?? new List<CatRelleno>(); }  catch { return new List<CatRelleno>(); }
        }

        public async Task<List<CatGlaseado>> GetGlaseadosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatGlaseado>>($"{_baseUrl}api/pastel/glaseados", GetJsonOptions()) ?? new List<CatGlaseado>(); }  catch { return new List<CatGlaseado>(); }
        }

		public async Task<List<Pastel>> GetPastelesAsync(bool? soloActivos = null)
		{
			try
			{
				var query = soloActivos.HasValue ? $"?soloActivos={soloActivos.Value}" : "";
				return await _httpClient.GetFromJsonAsync<List<Pastel>>($"{_baseUrl}api/pastel/todaspasteles{query}", GetJsonOptions()) ?? new List<Pastel>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al obtener pasteles: {ex.Message}");
				return new List<Pastel>();
			}
		}

        public async Task<Pastel?> GetPastelByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Pastel>($"{_baseUrl}api/pastel/{id}", GetJsonOptions());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pastel {id}: {ex.Message}");
                return null;
            }
        }

		public async Task<List<CatCategoria>> GetCategoriasAsync()
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<List<CatCategoria>>($"{_baseUrl}api/pastel/todascategorias", GetJsonOptions()) ?? new List<CatCategoria>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al obtener categorias: {ex.Message}");
				return new List<CatCategoria>();
			}
		}

        public async Task<bool> RegistrarCategoriaAsync(CatCategoria categoria)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevacategoria", categoria, GetJsonOptions());
                return response.IsSuccessStatusCode;
            }
             catch { return false; }
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarcategoria/{id}")).IsSuccessStatusCode; }
             catch { return false; }
        }

        public async Task<bool> RegistrarBizcochoAsync(CatBizcocho bizcocho)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevobizcocho", bizcocho, GetJsonOptions())).IsSuccessStatusCode; }
             catch { return false; }
        }
        public async Task<bool> EliminarBizcochoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarbizcocho/{id}")).IsSuccessStatusCode; }
             catch { return false; }
        }

        public async Task<bool> RegistrarRellenoAsync(CatRelleno relleno)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevorelleno", relleno, GetJsonOptions())).IsSuccessStatusCode; }
             catch { return false; }
        }
        public async Task<bool> EliminarRellenoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarrelleno/{id}")).IsSuccessStatusCode; }
             catch { return false; }
        }

        public async Task<bool> RegistrarGlaseadoAsync(CatGlaseado glaseado)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevoglaseado", glaseado, GetJsonOptions())).IsSuccessStatusCode; }
             catch { return false; }
        }
        public async Task<bool> EliminarGlaseadoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarglaseado/{id}")).IsSuccessStatusCode; }
             catch { return false; }
        }

		public async Task<bool> ActualizarPastelAsync(Pastel pastel)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/pastel/actualizarpastel", pastel, GetJsonOptions());
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al conectar con la API: {ex.Message}");
				return false;
			}
		}

		public async Task<bool> EliminarPastelAsync(int id)
		{
			try
			{
				// Ejecuta la baja lógica en la API
				var response = await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarpastel/{id}");
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al conectar con la API: {ex.Message}");
				return false;
			}
		}

		public async Task<bool> ReactivarPastelAsync(int id)
		{
			try
			{
				var response = await _httpClient.PutAsync($"{_baseUrl}api/pastel/reactivarpastel/{id}", null);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al reactivar pastel: {ex.Message}");
				return false;
			}
		}
	}
}

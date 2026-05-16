using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
	public class PastelApiService
	{
		private readonly HttpClient _httpClient;

		// URL base de la API adaptada según plataforma. localhost en Windows y 10.0.2.2 en Emulador Android.
		private readonly string _baseUrl = DeviceInfo.Platform == DevicePlatform.Android 
            ? "http://10.0.2.2:5105/api/pastel" 
            : "http://localhost:5105/api/pastel";

		public PastelApiService()
		{
			_httpClient = new HttpClient();
		}

		public async Task<Pastel> RegistrarPastelAsync(Pastel pastel)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/nuevopastel", pastel);
				if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Pastel>();
                }
                return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al conectar con la API: {ex.Message}");
				return null;
			}
		}

        public async Task<List<CatBizcocho>> GetBizcochosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatBizcocho>>($"{_baseUrl}/bizcochos"); } catch { return new List<CatBizcocho>(); }
        }

        public async Task<List<CatRelleno>> GetRellenosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatRelleno>>($"{_baseUrl}/rellenos"); } catch { return new List<CatRelleno>(); }
        }

        public async Task<List<CatGlaseado>> GetGlaseadosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatGlaseado>>($"{_baseUrl}/glaseados"); } catch { return new List<CatGlaseado>(); }
        }

		public async Task<List<Pastel>> GetPastelesAsync()
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<List<Pastel>>($"{_baseUrl}/todospasteles");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al obtener pasteles: {ex.Message}");
				return new List<Pastel>();
			}
		}

		public async Task<List<CatCategoria>> GetCategoriasAsync()
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<List<CatCategoria>>($"{_baseUrl}/todascategorias");
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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/nuevacategoria", categoria);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}/eliminarcategoria/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> RegistrarBizcochoAsync(CatBizcocho bizcocho)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}/nuevobizcocho", bizcocho)).IsSuccessStatusCode; }
            catch { return false; }
        }
        public async Task<bool> EliminarBizcochoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}/eliminarbizcocho/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> RegistrarRellenoAsync(CatRelleno relleno)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}/nuevorelleno", relleno)).IsSuccessStatusCode; }
            catch { return false; }
        }
        public async Task<bool> EliminarRellenoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}/eliminarrelleno/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> RegistrarGlaseadoAsync(CatGlaseado glaseado)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}/nuevoglaseado", glaseado)).IsSuccessStatusCode; }
            catch { return false; }
        }
        public async Task<bool> EliminarGlaseadoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}/eliminarglaseado/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

		public async Task<bool> ActualizarPastelAsync(Pastel pastel)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/actualizarpastel", pastel);
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
				var response = await _httpClient.DeleteAsync($"{_baseUrl}/eliminarpastel/{id}");
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al conectar con la API: {ex.Message}");
				return false;
			}
		}
	}
}
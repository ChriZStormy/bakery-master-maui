using System.Net.Http.Json;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
	public class PastelApiService
	{
		private readonly HttpClient _httpClient;

		private readonly string _baseUrl = "https://dolcevita-guh8gshvd0dre8d3.mexicocentral-01.azurewebsites.net/";

		public PastelApiService()
		{
			_httpClient = new HttpClient();
		}

		public async Task<Pastel> RegistrarPastelAsync(Pastel pastel)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevopastel", pastel);
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
            try { return await _httpClient.GetFromJsonAsync<List<CatBizcocho>>($"{_baseUrl}api/pastel/bizcochos"); } catch { return new List<CatBizcocho>(); }
        }

        public async Task<List<CatRelleno>> GetRellenosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatRelleno>>($"{_baseUrl}api/pastel/rellenos"); } catch { return new List<CatRelleno>(); }
        }

        public async Task<List<CatGlaseado>> GetGlaseadosAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<CatGlaseado>>($"{_baseUrl}api/pastel/glaseados"); } catch { return new List<CatGlaseado>(); }
        }

		public async Task<List<Pastel>> GetPastelesAsync()
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<List<Pastel>>($"{_baseUrl}api/pastel/todospasteles");
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
				return await _httpClient.GetFromJsonAsync<List<CatCategoria>>($"{_baseUrl}api/pastel/todascategorias");
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
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevacategoria", categoria);
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
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevobizcocho", bizcocho)).IsSuccessStatusCode; }
            catch { return false; }
        }
        public async Task<bool> EliminarBizcochoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarbizcocho/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> RegistrarRellenoAsync(CatRelleno relleno)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevorelleno", relleno)).IsSuccessStatusCode; }
            catch { return false; }
        }
        public async Task<bool> EliminarRellenoAsync(int id)
        {
            try { return (await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarrelleno/{id}")).IsSuccessStatusCode; }
            catch { return false; }
        }

        public async Task<bool> RegistrarGlaseadoAsync(CatGlaseado glaseado)
        {
            try { return (await _httpClient.PostAsJsonAsync($"{_baseUrl}api/pastel/nuevoglaseado", glaseado)).IsSuccessStatusCode; }
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
				var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}api/pastel/actualizarpastel", pastel);
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
				var response = await _httpClient.DeleteAsync($"{_baseUrl}api/pastel/eliminarpastel/{id}");
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

using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;

namespace PasteleriaAPI.Services
{
    public interface IPastelService
    {
		Task<List<Pastel>> GetAllPastelesAsync();
        Task<CatCategoria> SetCategoriaAsync(CatCategoria categoria );
        Task<List<CatCategoria>> SetVariasCategoriasAsync(List<CatCategoria> categorias);

        Task<List<Pastel>> GetAllPastelFromDBAsync();
        Task<List<CatCategoria>> GetAllCategoriasFromDBAsync();

        Task<bool> deletePastelAsync(int id);
        Task<Pastel> updatePastelAsync(Pastel pastel);
        Task<Pastel> SetPastelAsync(Pastel pastel);
	}
}

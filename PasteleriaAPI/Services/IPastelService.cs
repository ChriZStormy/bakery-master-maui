using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;

namespace PasteleriaAPI.Services
{
    public interface IPastelService
    {
        Task<List<Pastel>> GetAllPastelesAsync(bool soloActivos = false);
        Task<CatCategoria> SetCategoriaAsync(CatCategoria categoria);
        Task<List<CatCategoria>> SetVariasCategoriasAsync(List<CatCategoria> categorias);

        Task<List<Pastel>> GetAllPastelFromDBAsync(bool soloActivos = false);
        Task<List<CatCategoria>> GetAllCategoriasFromDBAsync();

        Task<bool> deletePastelAsync(int id);
        Task<bool> reactivarPastelAsync(int id);
        Task<Pastel> updatePastelAsync(Pastel pastel);
        Task<Pastel> SetPastelAsync(Pastel pastel);
    }
}

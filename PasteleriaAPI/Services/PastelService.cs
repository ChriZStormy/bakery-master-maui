using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;

namespace PasteleriaAPI.Services
{
    public class PastelService : IPastelService
    {
		private readonly ApplicationDbContext dbContext;

		public PastelService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<bool> deletePastelAsync(int id)
		{
			var pastelExistente = await dbContext.Pasteles.FindAsync(id);
			if (pastelExistente == null) return false;
			dbContext.Remove(pastelExistente);
			var pastelEliminado = await dbContext.SaveChangesAsync();
			return pastelEliminado > 0;
		}

		public async Task<List<Pastel>> GetAllPastelFromDBAsync()
		{
			return await dbContext.Pasteles
                .Include(p => p.Categoria)
                .Include(p => p.Bizcocho)
                .Include(p => p.Relleno)
                .Include(p => p.Glaseado)
                .Where(p => !p.IsPersonalizado)
                .ToListAsync();
		}

		public async Task<List<CatCategoria>> GetAllCategoriasFromDBAsync()
		{
			return await dbContext.CatCategorias.ToListAsync();
		}

		public async Task<List<Pastel>> GetAllPastelesAsync()
        {
            return await dbContext.Pasteles
                .Include(p => p.Categoria)
                .Include(p => p.Bizcocho)
                .Include(p => p.Relleno)
                .Include(p => p.Glaseado)
                .Where(p => !p.IsPersonalizado)
                .ToListAsync();
		}

		public async Task<CatCategoria> SetCategoriaAsync(CatCategoria categoria)
		{
			await dbContext.CatCategorias.AddAsync(categoria);
			var nuevaCategoriaGuardada = await dbContext.SaveChangesAsync();
			return nuevaCategoriaGuardada > 0 ? categoria : null;

		}

		public async Task<List<CatCategoria>> SetVariasCategoriasAsync(List<CatCategoria> categorias)
		{
			await dbContext.AddRangeAsync(categorias);
			var nuevasCategoriasGuardadas = await dbContext.SaveChangesAsync();
			return nuevasCategoriasGuardadas > 0 ? categorias : null;



		}

		public async Task<Pastel> SetPastelAsync(Pastel pastel)
		{
			await dbContext.Pasteles.AddAsync(pastel);
			var nuevoPastelGuardado = await dbContext.SaveChangesAsync();
			return nuevoPastelGuardado > 0 ? pastel : null;
		}

		public async Task<Pastel> updatePastelAsync(Pastel pastel)
		{
			var pastelExistente = await dbContext.Pasteles.FindAsync(pastel.Id);
			if (pastelExistente == null) return null;
			
			pastelExistente.Nombre = pastel.Nombre;
			pastelExistente.Tamanio = pastel.Tamanio;
			pastelExistente.CategoriaId = pastel.CategoriaId;
            pastelExistente.BizcochoId = pastel.BizcochoId;
            pastelExistente.RellenoId = pastel.RellenoId;
            pastelExistente.GlaseadoId = pastel.GlaseadoId;
            pastelExistente.FotoUrl = pastel.FotoUrl;
			
			dbContext.Pasteles.Update(pastelExistente);
			var pastelEditado = await dbContext.SaveChangesAsync();
			return pastelEditado > 0 ? pastelExistente : null;
		}
	}
}

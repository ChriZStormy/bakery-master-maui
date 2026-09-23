using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Entities.Catalogos;
using PasteleriaAPI.Services;

namespace PasteleriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PastelController : ControllerBase
    {
        private readonly IPastelService pastelSercvice;

        public PastelController(IPastelService pastelSercvice)
        {
            this.pastelSercvice = pastelSercvice;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pastel>>> GetAllPasteles()
        {
            // Delegamos la obtención de los datos a la capa de servicios (IPastelService) 
            // para mantener el controlador limpio y separar la lógica de acceso a datos.

            var pasteles = await pastelSercvice.GetAllPastelesAsync();


            if (pasteles.Count == 0)
            {

                return NoContent();
            }
            else
            {
                return Ok(pasteles);
            }
        }

        [HttpPost("nuevacategoria")]
        public async Task<ActionResult<CatCategoria>> SetCategoria([FromBody] CatCategoria categoria)
        {
            var nuevaCategoria = await pastelSercvice.SetCategoriaAsync(categoria);
            if (nuevaCategoria == null)
            {
                return BadRequest("No se pudo guardar la categoria");
            }
            else
            {
                return Ok(nuevaCategoria);
            }

        }

        [HttpPost("nuevopastel")]
        public async Task<ActionResult<Pastel>> SetPastel([FromBody] Pastel pastel)
        {
            var nuevoPastel = await pastelSercvice.SetPastelAsync(pastel);
            if (nuevoPastel == null)
            {
                return BadRequest("No se pudo guardar el pastel");
            }
            else
            {
                return Ok(nuevoPastel);
            }
        }

        [HttpGet("todospasteles")]
        public async Task<ActionResult<List<Pastel>>> GetAllPastelesFromDB()
        {
            var pasteles = await pastelSercvice.GetAllPastelFromDBAsync();
            if (pasteles.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(pasteles);
            }
        }

        [HttpGet("todascategorias")]
        public async Task<ActionResult<List<CatCategoria>>> GetAllCategoriasFromDB()
        {
            var categorias = await pastelSercvice.GetAllCategoriasFromDBAsync();
            if (categorias.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(categorias);
            }
        }

        [HttpDelete("eliminarcategoria/{id}")]
        public async Task<ActionResult<bool>> DeleteCategoria(int id, [FromServices] ApplicationDbContext db)
        {
            var item = await db.CatCategorias.FindAsync(id);
            if (item == null) return BadRequest("No se pudo encontrar la categoría.");
            db.CatCategorias.Remove(item);
            await db.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPut("actualizarcategoria")]
        public async Task<ActionResult<CatCategoria>> UpdateCategoria([FromBody] CatCategoria categoria, [FromServices] ApplicationDbContext db)
        {
            db.CatCategorias.Update(categoria);
            await db.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpPost("nuevobizcocho")]

        // Endpoint para administradores. Inyectamos directamente el ApplicationDbContext 
        // para agregar rápidamente un nuevo elemento al catálogo dinámico de bizcochos.
        public async Task<ActionResult<CatBizcocho>> SetBizcocho([FromBody] CatBizcocho bizcocho, [FromServices] ApplicationDbContext db)
        {
            db.CatBizcochos.Add(bizcocho);
            await db.SaveChangesAsync();
            return Ok(bizcocho);
        }

        [HttpDelete("eliminarbizcocho/{id}")]
        public async Task<ActionResult<bool>> DeleteBizcocho(int id, [FromServices] ApplicationDbContext db)
        {
            var item = await db.CatBizcochos.FindAsync(id);
            if (item == null) return BadRequest("No existe");
            db.CatBizcochos.Remove(item);
            await db.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPut("actualizarbizcocho")]
        public async Task<ActionResult<CatBizcocho>> UpdateBizcocho([FromBody] CatBizcocho bizcocho, [FromServices] ApplicationDbContext db)
        {
            db.CatBizcochos.Update(bizcocho);
            await db.SaveChangesAsync();
            return Ok(bizcocho);
        }

        [HttpPost("nuevorelleno")]
        public async Task<ActionResult<CatRelleno>> SetRelleno([FromBody] CatRelleno relleno, [FromServices] ApplicationDbContext db)
        {
            db.CatRellenos.Add(relleno);
            await db.SaveChangesAsync();
            return Ok(relleno);
        }

        [HttpDelete("eliminarrelleno/{id}")]
        public async Task<ActionResult<bool>> DeleteRelleno(int id, [FromServices] ApplicationDbContext db)
        {
            var item = await db.CatRellenos.FindAsync(id);
            if (item == null) return BadRequest("No existe");
            db.CatRellenos.Remove(item);
            await db.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPut("actualizarrelleno")]
        public async Task<ActionResult<CatRelleno>> UpdateRelleno([FromBody] CatRelleno relleno, [FromServices] ApplicationDbContext db)
        {
            db.CatRellenos.Update(relleno);
            await db.SaveChangesAsync();
            return Ok(relleno);
        }

        [HttpPost("nuevoglaseado")]
        public async Task<ActionResult<CatGlaseado>> SetGlaseado([FromBody] CatGlaseado glaseado, [FromServices] ApplicationDbContext db)
        {
            db.CatGlaseados.Add(glaseado);
            await db.SaveChangesAsync();
            return Ok(glaseado);
        }

        [HttpDelete("eliminarglaseado/{id}")]
        public async Task<ActionResult<bool>> DeleteGlaseado(int id, [FromServices] ApplicationDbContext db)
        {
            var item = await db.CatGlaseados.FindAsync(id);
            if (item == null) return BadRequest("No existe");
            db.CatGlaseados.Remove(item);
            await db.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPut("actualizarglaseado")]
        public async Task<ActionResult<CatGlaseado>> UpdateGlaseado([FromBody] CatGlaseado glaseado, [FromServices] ApplicationDbContext db)
        {
            db.CatGlaseados.Update(glaseado);
            await db.SaveChangesAsync();
            return Ok(glaseado);
        }

        [HttpDelete("eliminarpastel/{id}")]
        public async Task<ActionResult<bool>> DeletePastel(int id)
        {
            var pastelEliminado = await pastelSercvice.deletePastelAsync(id);
            if (!pastelEliminado)
            {
                return BadRequest("No se pudo eliminar el pastel. ");
            }
            else
            {
                return Ok("Pastel Eliminado Existosamente");
            }
        }


        [HttpPut("actualizarpastel")]
        public async Task<ActionResult<Pastel>> UpdatePastel([FromBody] Pastel pastel)
        {
            var pastelActualizado = await pastelSercvice.updatePastelAsync(pastel);
            if (pastelActualizado == null)
            {
                return BadRequest("No se pudo actualizar el pastel");
            }
            else
            {
                return Ok(pastelActualizado);
            }
        }

        [HttpGet("bizcochos")]
        public async Task<ActionResult<List<CatBizcocho>>> GetBizcochos([FromServices] ApplicationDbContext db)
        {
            return Ok(await db.CatBizcochos.ToListAsync());
        }

        [HttpGet("rellenos")]
        public async Task<ActionResult<List<CatRelleno>>> GetRellenos([FromServices] ApplicationDbContext db)
        {
            return Ok(await db.CatRellenos.ToListAsync());
        }

        [HttpGet("glaseados")]
        public async Task<ActionResult<List<CatGlaseado>>> GetGlaseados([FromServices] ApplicationDbContext db)
        {
            return Ok(await db.CatGlaseados.ToListAsync());
        }
    }
}

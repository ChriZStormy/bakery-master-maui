using System.Globalization;
using System.Security;

namespace PasteleriaAPI.Entities.Catalogos
{
    public class CatCategoria
    {
       public int Id { get; set; }
       public string? categoria { get; set; }

        public ICollection<Pastel>? Pasteles { get; set; }
	}
}

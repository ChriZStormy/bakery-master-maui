namespace PasteleriaMaui.Models
{
	public class Pastel
	{
		public int Id { get; set; }
		public int CategoriaId { get; set; }
        public CatCategoria? Categoria { get; set; }

		public string Nombre { get; set; } = string.Empty;
		public string Tamanio { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public string? Descripcion { get; set; }
        public string? Ingredientes { get; set; }
        public string? Alergenos { get; set; }
        public string? FotoUrl { get; set; }
        public string? FotosUrls { get; set; }
        public string? Mensaje { get; set; }
        public string? ImagenDecoracion { get; set; }
        public bool IsPersonalizado { get; set; }
        public bool Activo { get; set; } = true;
        public bool Agotado { get; set; } = false;

        public int BizcochoId { get; set; }
        public int RellenoId { get; set; }
        public int GlaseadoId { get; set; }

        public CatBizcocho? Bizcocho { get; set; }
        public CatRelleno? Relleno { get; set; }
        public CatGlaseado? Glaseado { get; set; }

        public string PrecioBaseFormateado => PrecioBase > 0 ? $"Desde ${PrecioBase:N2}" : "Desde $350.00";
	}
}

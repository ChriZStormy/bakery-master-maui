namespace PasteleriaMaui.Models
{
	public class Pastel
	{
		public int Id { get; set; }
		public int CategoriaId { get; set; }
        public CatCategoria? Categoria { get; set; }

		public string Nombre { get; set; } = string.Empty;
		public string Tamanio { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public string? Mensaje { get; set; }
        public string? ImagenDecoracion { get; set; }
        public bool IsPersonalizado { get; set; }
        public bool Activo { get; set; } = true;

        public int BizcochoId { get; set; }
        public int RellenoId { get; set; }
        public int GlaseadoId { get; set; }

        public CatBizcocho? Bizcocho { get; set; }
        public CatRelleno? Relleno { get; set; }
        public CatGlaseado? Glaseado { get; set; }
	}
}

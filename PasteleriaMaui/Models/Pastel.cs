namespace PasteleriaMaui.Models
{
	public class Pastel
	{
		public int Id { get; set; } 
		public int CategoriaId { get; set; }
        public CatCategoria Categoria { get; set; }

		public string Nombre { get; set; }
		public string Tamanio { get; set; }
        public string FotoUrl { get; set; }
        public bool IsPersonalizado { get; set; }

        public int BizcochoId { get; set; }
        public int RellenoId { get; set; }
        public int GlaseadoId { get; set; }

        public CatBizcocho Bizcocho { get; set; }
        public CatRelleno Relleno { get; set; }
        public CatGlaseado Glaseado { get; set; }
	}
}

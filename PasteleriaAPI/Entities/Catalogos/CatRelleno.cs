namespace PasteleriaAPI.Entities.Catalogos
{
    public class CatRelleno
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal CostoAdicional { get; set; } = 0m;
        public bool Activo { get; set; } = true;
        public bool Agotado { get; set; } = false;
    }
}

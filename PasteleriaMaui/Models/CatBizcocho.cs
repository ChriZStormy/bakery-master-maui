namespace PasteleriaMaui.Models
{
    public class CatBizcocho
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal CostoAdicional { get; set; } = 0m;
        public bool Activo { get; set; } = true;
        public bool Agotado { get; set; } = false;

        public string CardImagenUrl => (Nombre ?? "").ToLowerInvariant() switch
        {
            var n when n.Contains("vainilla") => "card_bizcocho_vainilla.jpg",
            var n when n.Contains("chocolate") => "card_bizcocho_chocolate.jpg",
            var n when n.Contains("red velvet") => "card_bizcocho_red_velvet.jpg",
            var n when n.Contains("zanahoria") => "card_bizcocho_zanahoria.jpg",
            var n when n.Contains("mármol") || n.Contains("marmol") => "card_bizcocho_marmol.jpg",
            var n when n.Contains("limón") || n.Contains("limon") => "card_bizcocho_limon.jpg",
            var n when n.Contains("naranja") => "card_bizcocho_naranja.jpg",
            var n when n.Contains("esponja") => "card_bizcocho_esponja.jpg",
            _ => "bizcocho_editorial.jpg"
        };
    }
}

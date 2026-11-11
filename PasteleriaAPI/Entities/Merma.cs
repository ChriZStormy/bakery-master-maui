using System;

namespace PasteleriaAPI.Entities
{
    public class Merma
    {
        public int Id { get; set; }
        public int? PastelId { get; set; }
        public string TipoElemento { get; set; } = "Pastel"; // "Pastel" o "Insumo"
        public string ElementoAfectado { get; set; } = string.Empty;
        public double Cantidad { get; set; } = 1;
        public string UnidadMedida { get; set; } = "Piezas"; // "Piezas", "Kg", "Litros"
        public string Causa { get; set; } = "Caducidad de insumo"; // "Caducidad de insumo", "Error de horneado/quemado", "Daño durante decoración/transporte", "Otro"
        public decimal CostoEstimado { get; set; } = 0m;
        public string? UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Descripcion { get; set; } // Observación opcional
        public string? Estatus { get; set; } = "Registrada";
        
        public Pastel? Pastel { get; set; }
        public ApplicationUser? Usuario { get; set; }
    }
}

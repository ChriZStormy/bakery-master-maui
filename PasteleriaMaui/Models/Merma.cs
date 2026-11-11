using System;
using System.Globalization;

namespace PasteleriaMaui.Models
{
    public class Merma
    {
        public int Id { get; set; }
        public int? PastelId { get; set; }
        public string TipoElemento { get; set; } = "Pastel"; // "Pastel" o "Insumo"
        public string ElementoAfectado { get; set; } = string.Empty;
        public double Cantidad { get; set; } = 1;
        public string UnidadMedida { get; set; } = "Piezas"; // "Piezas", "Kg", "Litros"
        public string Causa { get; set; } = "Caducidad de insumo"; // 'Caducidad de insumo', 'Error de horneado/quemado', 'Daño durante decoración/transporte', 'Otro'
        public decimal CostoEstimado { get; set; } = 0m;
        public string? UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Descripcion { get; set; }
        public string Estatus { get; set; } = "Registrada";

        public Pastel? Pastel { get; set; }
        public Usuario? Usuario { get; set; }

        public string ElementoDisplay => !string.IsNullOrWhiteSpace(ElementoAfectado)
            ? ElementoAfectado
            : (Pastel?.Nombre ?? "Insumo o Pastel general");

        public string CantidadDisplay => $"{Cantidad:0.##} {UnidadMedida}";

        public string CostoDisplay => CostoEstimado.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        public string FechaDisplay => Fecha.ToString("dd/MM/yyyy HH:mm");

        public string CausaBadgeColor => Causa switch
        {
            "Caducidad de insumo" => "#E65100",
            "Error de horneado/quemado" => "#C62828",
            "Daño durante decoración/transporte" => "#AD1457",
            _ => "#4E342E"
        };
    }
}

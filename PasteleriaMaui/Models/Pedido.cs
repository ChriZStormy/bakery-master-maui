using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace PasteleriaMaui.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PastelId { get; set; }
        public string? UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public string Estatus { get; set; } = "Pendiente";
        public string? Mensaje { get; set; }
        public string? ImagenDecoracion { get; set; }

        public string? HoraEntrega { get; set; }
        public string? ModalidadEntrega { get; set; }
        public string? Direccion { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public DateTime? FechaRecibido { get; set; }
        public DateTime? FechaPreparacion { get; set; }
        public DateTime? FechaListo { get; set; }
        public DateTime? FechaEntregado { get; set; }

        [JsonIgnore]
        public string? Hora
        {
            get => HoraEntrega;
            set => HoraEntrega = value;
        }

        [JsonIgnore]
        public string? Modalidad
        {
            get => ModalidadEntrega;
            set => ModalidadEntrega = value;
        }

        public Pastel? Pastel { get; set; }
        public Usuario? Usuario { get; set; }

        // Propiedades de apoyo para reportes y visualización
        [JsonIgnore]
        public string TotalFormateado => Total.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        [JsonIgnore]
        public bool EsATiempo
        {
            get
            {
                DateTime entregaEfectiva = FechaEntregaReal ?? FechaEntrega;
                return entregaEfectiva <= FechaEntrega.AddHours(2);
            }
        }

        [JsonIgnore]
        public string EstadoPuntualidadTexto
        {
            get
            {
                if (Estatus == "Entregado" || Estatus == "Completado")
                {
                    return EsATiempo ? "A Tiempo" : "Retrasado";
                }
                if (DateTime.Now > FechaEntrega)
                {
                    return "Retrasado";
                }
                return "En Preparación / Ruta";
            }
        }

        [JsonIgnore]
        public string EstadoPuntualidadColor => EstadoPuntualidadTexto switch
        {
            "A Tiempo" => "#2E7D32",
            "Retrasado" => "#C62828",
            _ => "#E65100"
        };

        [JsonIgnore]
        public double HorasPreparacion
        {
            get
            {
                DateTime fin = FechaEntregaReal ?? (Estatus == "Entregado" ? FechaEntrega : DateTime.Now);
                var diff = (fin - Fecha).TotalHours;
                return diff > 0 ? Math.Round(diff, 1) : 1.0;
            }
        }
    }
}

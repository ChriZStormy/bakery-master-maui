using System;
namespace PasteleriaMaui.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PastelId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public string Estatus { get; set; }
        public Pastel Pastel { get; set; }
        public Usuario Usuario { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool PuedeIniciarElaboracion => 
            (AppSession.CurrentUser?.Rol == "Repostero" || AppSession.CurrentUser?.Rol == "Empleado/Repostero" || AppSession.CurrentUser?.Rol == "Admin") && 
            Estatus == "Pendiente";

        [System.Text.Json.Serialization.JsonIgnore]
        public bool PuedeFinalizarMontaje => 
            (AppSession.CurrentUser?.Rol == "Repostero" || AppSession.CurrentUser?.Rol == "Empleado/Repostero" || AppSession.CurrentUser?.Rol == "Admin") && 
            (Estatus == "En Preparación" || Estatus == "En Proceso");

        [System.Text.Json.Serialization.JsonIgnore]
        public string EstatusBadgeColor => Estatus switch
        {
            "Pendiente" => "#E65100",
            "En Preparación" => "#1565C0",
            "En Proceso" => "#1565C0",
            "Listo para entrega" => "#2E7D32",
            "Entregado" => "#4E342E",
            "Cancelado" => "#C62828",
            _ => "#8B5E34"
        };

        [System.Text.Json.Serialization.JsonIgnore]
        public string EstatusBadgeBg => Estatus switch
        {
            "Pendiente" => "#FFF3E0",
            "En Preparación" => "#E3F2FD",
            "En Proceso" => "#E3F2FD",
            "Listo para entrega" => "#E8F5E9",
            "Entregado" => "#EFEBE9",
            "Cancelado" => "#FFEBEE",
            _ => "#FAF8F5"
        };
    }
}

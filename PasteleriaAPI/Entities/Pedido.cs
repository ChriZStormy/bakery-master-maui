using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteleriaAPI.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PastelId { get; set; }
        public string? UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public string? Estatus { get; set; }
        public string? Mensaje { get; set; }
        public string? ImagenDecoracion { get; set; }

        public string? HoraEntrega { get; set; }
        public string? ModalidadEntrega { get; set; }
        public string? Direccion { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }

        [NotMapped]
        public string? Hora
        {
            get => HoraEntrega;
            set => HoraEntrega = value;
        }

        [NotMapped]
        public string? Modalidad
        {
            get => ModalidadEntrega;
            set => ModalidadEntrega = value;
        }
        
        public Pastel? Pastel { get; set; }
        public ApplicationUser? Usuario { get; set; }
    }
}

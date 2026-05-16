using System;
namespace PasteleriaAPI.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PastelId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public string? Estatus { get; set; }
        
        public Pastel? Pastel { get; set; }
        public Usuario? Usuario { get; set; }
    }
}

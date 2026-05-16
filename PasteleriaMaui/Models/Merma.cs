using System;
namespace PasteleriaMaui.Models
{
    public class Merma
    {
        public int Id { get; set; }
        public int PastelId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public string Estatus { get; set; } = "Pendiente";
        public Pastel Pastel { get; set; }
        public Usuario Usuario { get; set; }
    }
}

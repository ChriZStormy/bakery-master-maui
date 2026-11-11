using System.Collections.Generic;

namespace PasteleriaMaui.Models
{
    public class ReporteEntregasDto
    {
        public int TotalPedidos { get; set; }
        public int EntregadosATiempo { get; set; }
        public int EntregasRetrasadas { get; set; }
        public int EnProceso { get; set; }
        public double TiempoPromedioPreparacionHoras { get; set; }
        public double PorcentajePuntualidad { get; set; }
        public List<Pedido> Pedidos { get; set; } = new();
    }
}

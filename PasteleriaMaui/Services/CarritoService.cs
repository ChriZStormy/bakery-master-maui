using System.Collections.ObjectModel;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Services
{
    public class CarritoItem
    {
        public Pastel Pastel { get; set; } = new();
        public string Porciones { get; set; } = "10 personas";
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }
        public string? Dedicatoria { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
        public string SubtotalFormateado => $"${Subtotal:N2} MXN";
    }

    public class CarritoService
    {
        public static CarritoService Instance { get; } = new CarritoService();

        public ObservableCollection<CarritoItem> Items { get; } = new();

        public decimal Total => Items.Sum(i => i.Subtotal);

        public void Agregar(CarritoItem item)
        {
            Items.Add(item);
        }

        public void Eliminar(CarritoItem item)
        {
            Items.Remove(item);
        }

        public void Limpiar()
        {
            Items.Clear();
        }
    }
}

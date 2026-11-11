using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Views;

namespace PasteleriaMaui.ViewModels
{
    [QueryProperty(nameof(PedidoComprobante), "PedidoComprobante")]
    public class ComprobantePedidoViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private Pedido? _pedidoComprobante;
        public Pedido? PedidoComprobante
        {
            get => _pedidoComprobante;
            set
            {
                if (_pedidoComprobante != value)
                {
                    _pedidoComprobante = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FolioFormateado));
                    OnPropertyChanged(nameof(FechaCreacionFormateada));
                    OnPropertyChanged(nameof(FechaEntregaFormateada));
                    OnPropertyChanged(nameof(HoraFormateada));
                    OnPropertyChanged(nameof(NombrePastel));
                    OnPropertyChanged(nameof(TamanioPastel));
                    OnPropertyChanged(nameof(FotoPastel));
                    OnPropertyChanged(nameof(Modalidad));
                    OnPropertyChanged(nameof(Direccion));
                    OnPropertyChanged(nameof(Cantidad));
                    OnPropertyChanged(nameof(Estatus));
                    OnPropertyChanged(nameof(TotalFormateado));
                }
            }
        }

        public string FolioFormateado => PedidoComprobante?.Id > 0 
            ? $"#PED-{PedidoComprobante.Id:D5}" 
            : "#PED-ORDEN";

        public string FechaCreacionFormateada => (PedidoComprobante?.Fecha ?? DateTime.Now).ToString("dd/MM/yyyy HH:mm");
        public string FechaEntregaFormateada => (PedidoComprobante?.FechaEntrega ?? DateTime.Today).ToString("dddd, dd 'de' MMMM 'de' yyyy", CultureInfo.GetCultureInfo("es-MX"));
        public string HoraFormateada => PedidoComprobante?.HoraEntrega ?? "Por definir";

        public string NombrePastel => PedidoComprobante?.Pastel?.Nombre ?? "Pastel Artesanal";
        public string TamanioPastel => PedidoComprobante?.Pastel?.Tamanio ?? "Mediano";
        public string FotoPastel => !string.IsNullOrEmpty(PedidoComprobante?.Pastel?.FotoUrl) ? PedidoComprobante.Pastel.FotoUrl : "dotnet_bot.png";

        public string Modalidad => PedidoComprobante?.ModalidadEntrega ?? "Recolección en Tienda";
        public string Direccion => !string.IsNullOrEmpty(PedidoComprobante?.Direccion) ? PedidoComprobante.Direccion : "Recolección en sucursal";
        public int Cantidad => PedidoComprobante?.Cantidad ?? 1;
        public string Estatus => PedidoComprobante?.Estatus ?? "Pendiente";

        public string TotalFormateado => (PedidoComprobante?.Total ?? 0).ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        public ICommand VerHistorialCommand { get; }
        public ICommand VolverAlInicioCommand { get; }

        public ComprobantePedidoViewModel()
        {
            VerHistorialCommand = new Command(async () => await VerHistorial());
            VolverAlInicioCommand = new Command(async () => await VolverAlInicio());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("PedidoComprobante", out var pedidoObj) && pedidoObj is Pedido p)
            {
                PedidoComprobante = p;
            }
        }

        private async Task VerHistorial()
        {
            await Shell.Current.GoToAsync(nameof(HistorialPedidosPage));
        }

        private async Task VolverAlInicio()
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using PasteleriaMaui.Views;

namespace PasteleriaMaui.ViewModels
{
    [QueryProperty(nameof(PastelAComprar), "PastelAComprar")]
    public class RegistrarPedidoViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private readonly TransaccionesService _transaccionesService;

        private Pastel? _pastelAComprar;
        public Pastel? PastelAComprar
        {
            get => _pastelAComprar;
            set
            {
                if (_pastelAComprar != value)
                {
                    _pastelAComprar = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(NombrePastel));
                    OnPropertyChanged(nameof(TamanioPastel));
                    OnPropertyChanged(nameof(FotoPastel));
                    RecalcularCostos();
                }
            }
        }

        public string NombrePastel => PastelAComprar?.Nombre ?? "Pastel Seleccionado";
        public string TamanioPastel => PastelAComprar?.Tamanio ?? "Mediano";
        public string FotoPastel => !string.IsNullOrEmpty(PastelAComprar?.FotoUrl) ? PastelAComprar.FotoUrl : "dotnet_bot.png";

        private string _cantidad = "1";
        public string Cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged();
                    RecalcularCostos();
                }
            }
        }

        private DateTime _fechaEntrega = DateTime.Today.AddDays(2);
        public DateTime FechaEntrega
        {
            get => _fechaEntrega;
            set
            {
                if (_fechaEntrega != value)
                {
                    _fechaEntrega = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime FechaMinima => DateTime.Today;

        private TimeSpan _horaEntrega = new TimeSpan(14, 0, 0);
        public TimeSpan HoraEntrega
        {
            get => _horaEntrega;
            set
            {
                if (_horaEntrega != value)
                {
                    _horaEntrega = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HoraFormateada));
                }
            }
        }

        public string HoraFormateada => DateTime.Today.Add(HoraEntrega).ToString("hh:mm tt");

        public ObservableCollection<string> ModalidadesEntrega { get; } = new()
        {
            "Recolección en Tienda",
            "Envío a Domicilio"
        };

        private string _modalidadSeleccionada = "Recolección en Tienda";
        public string ModalidadSeleccionada
        {
            get => _modalidadSeleccionada;
            set
            {
                if (_modalidadSeleccionada != value)
                {
                    _modalidadSeleccionada = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EsEnvioADomicilio));
                    RecalcularCostos();
                }
            }
        }

        public bool EsEnvioADomicilio => ModalidadSeleccionada == "Envío a Domicilio";

        private string _direccion = string.Empty;
        public string Direccion
        {
            get => _direccion;
            set
            {
                if (_direccion != value)
                {
                    _direccion = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _precioUnitario = 380m;
        public decimal PrecioUnitario
        {
            get => _precioUnitario;
            private set
            {
                if (_precioUnitario != value)
                {
                    _precioUnitario = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PrecioUnitarioFormateado));
                }
            }
        }

        public string PrecioUnitarioFormateado => PrecioUnitario.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        private decimal _costoEnvio = 0m;
        public decimal CostoEnvio
        {
            get => _costoEnvio;
            private set
            {
                if (_costoEnvio != value)
                {
                    _costoEnvio = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CostoEnvioFormateado));
                }
            }
        }

        public string CostoEnvioFormateado => CostoEnvio.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        private decimal _subtotal = 380m;
        public decimal Subtotal
        {
            get => _subtotal;
            private set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SubtotalFormateado));
                }
            }
        }

        public string SubtotalFormateado => Subtotal.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        private decimal _total = 380m;
        public decimal Total
        {
            get => _total;
            private set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalFormateado));
                }
            }
        }

        public string TotalFormateado => Total.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        public ICommand ConfirmarPedidoCommand { get; }

        public RegistrarPedidoViewModel()
        {
            _transaccionesService = new TransaccionesService();
            ConfirmarPedidoCommand = new Command(async () => await ConfirmarPedido());
            RecalcularCostos();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("PastelAComprar", out var pastelObj) && pastelObj is Pastel pastel)
            {
                PastelAComprar = pastel;
            }
        }

        private void RecalcularCostos()
        {
            var tamanio = PastelAComprar?.Tamanio?.Trim().ToLowerInvariant() ?? "mediano";
            PrecioUnitario = tamanio switch
            {
                "chico" => 250.00m,
                "mediano" => 380.00m,
                "grande" => 520.00m,
                "extra grande" => 680.00m,
                _ => 380.00m
            };

            int qty = 1;
            if (int.TryParse(Cantidad, out int parsedQty) && parsedQty > 0)
            {
                qty = parsedQty;
            }

            CostoEnvio = EsEnvioADomicilio ? 50.00m : 0.00m;
            Subtotal = PrecioUnitario * qty;
            Total = Subtotal + CostoEnvio;
        }

        private async Task ConfirmarPedido()
        {
            if (PastelAComprar == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se ha seleccionado ningún pastel.", "OK");
                return;
            }

            if (!int.TryParse(Cantidad, out int cantidad) || cantidad <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Cantidad Inválida", "La cantidad de pasteles debe ser mayor a cero.", "OK");
                return;
            }

            if (EsEnvioADomicilio && string.IsNullOrWhiteSpace(Direccion))
            {
                await Application.Current.MainPage.DisplayAlert("Dirección Requerida", "Por favor ingresa la dirección completa para el envío a domicilio.", "OK");
                return;
            }

            var horaTexto = DateTime.Today.Add(HoraEntrega).ToString("hh:mm tt");
            var direccionFinal = EsEnvioADomicilio ? Direccion.Trim() : "Recolección en sucursal principal";

            var nuevoPedido = new Pedido
            {
                PastelId = PastelAComprar.Id,
                UsuarioId = AppSession.CurrentUser?.Id,
                Cantidad = cantidad,
                Fecha = DateTime.Now,
                FechaEntrega = FechaEntrega.Date,
                HoraEntrega = horaTexto,
                ModalidadEntrega = ModalidadSeleccionada,
                Direccion = direccionFinal,
                Total = Total,
                Estatus = "Pendiente",
                Pastel = PastelAComprar
            };

            var (exito, pedidoCreado) = await _transaccionesService.RegistrarPedidoAsync(nuevoPedido);
            if (exito)
            {
                var pedidoParaComprobante = pedidoCreado ?? nuevoPedido;
                if (pedidoParaComprobante.Pastel == null)
                {
                    pedidoParaComprobante.Pastel = PastelAComprar;
                }

                var navParam = new Dictionary<string, object>
                {
                    { "PedidoComprobante", pedidoParaComprobante }
                };

                await Shell.Current.GoToAsync(nameof(ComprobantePedidoPage), navParam);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Hubo un problema al registrar tu pedido. Por favor intenta de nuevo.", "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

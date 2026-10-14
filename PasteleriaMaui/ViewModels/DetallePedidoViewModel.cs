using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    public class DetallePedidoViewModel : INotifyPropertyChanged
    {
        private readonly TransaccionesService _transaccionesService;
        private Pedido _pedido;
        private string _nuevoEstatus;

        public Pedido Pedido
        {
            get => _pedido;
            set 
            { 
                _pedido = value; 
                OnPropertyChanged();
                ActualizarPropiedadesFase();
            }
        }

        public string NuevoEstatus
        {
            get => _nuevoEstatus;
            set { _nuevoEstatus = value; OnPropertyChanged(); }
        }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";
        public bool IsRepostero => AppSession.CurrentUser?.Rol == "Repostero" || AppSession.CurrentUser?.Rol == "Empleado/Repostero";
        public bool CanActualizarEstatus => IsAdmin || IsRepostero;

        public decimal MontoTotal
        {
            get
            {
                decimal basePrice = 350m;
                if (Pedido?.Pastel?.Tamanio != null)
                {
                    basePrice = Pedido.Pastel.Tamanio.Trim().ToLower() switch
                    {
                        "chico" => 250m,
                        "mediano" => 350m,
                        "grande" => 480m,
                        "extra grande" => 620m,
                        _ => 350m
                    };
                }
                int cant = Pedido?.Cantidad > 0 ? Pedido.Cantidad : 1;
                return basePrice * cant;
            }
        }

        public int FaseActual
        {
            get
            {
                var e = Pedido?.Estatus?.Trim().ToLower() ?? "";
                if (e.Contains("entregad")) return 4;
                if (e.Contains("listo")) return 3;
                if (e.Contains("preparaci") || e.Contains("proceso")) return 2;
                return 1; // Pendiente
            }
        }

        public bool Fase1Completada => FaseActual >= 1;
        public bool Fase2Completada => FaseActual >= 2;
        public bool Fase3Completada => FaseActual >= 3;
        public bool Fase4Completada => FaseActual >= 4;

        public bool IsFasePendiente => FaseActual == 1;
        public bool IsFasePreparacion => FaseActual == 2;
        public bool IsFaseListo => FaseActual == 3;
        public bool IsFaseEntregado => FaseActual == 4;

        public ICommand VolverCommand { get; set; }
        public ICommand ActualizarEstatusCommand { get; set; }
        public ICommand IniciarElaboracionCommand { get; set; }
        public ICommand FinalizarMontajeCommand { get; set; }
        public ICommand MarcarEntregadoCommand { get; set; }

        public DetallePedidoViewModel(Pedido pedidoSeleccionado, INavigation navigation)
        {
            _transaccionesService = new TransaccionesService();
            Pedido = pedidoSeleccionado;
            NuevoEstatus = Pedido?.Estatus ?? "Pendiente";

            VolverCommand = new Command(async () => await navigation.PopAsync());
            ActualizarEstatusCommand = new Command(async () => await ActualizarEstatus());

            IniciarElaboracionCommand = new Command(async () => 
            {
                NuevoEstatus = "En Preparación";
                await ActualizarEstatus();
            });

            FinalizarMontajeCommand = new Command(async () => 
            {
                NuevoEstatus = "Listo para entrega";
                await ActualizarEstatus();
            });

            MarcarEntregadoCommand = new Command(async () => 
            {
                NuevoEstatus = "Entregado";
                await ActualizarEstatus();
            });
        }

        private void ActualizarPropiedadesFase()
        {
            OnPropertyChanged(nameof(FaseActual));
            OnPropertyChanged(nameof(Fase1Completada));
            OnPropertyChanged(nameof(Fase2Completada));
            OnPropertyChanged(nameof(Fase3Completada));
            OnPropertyChanged(nameof(Fase4Completada));
            OnPropertyChanged(nameof(IsFasePendiente));
            OnPropertyChanged(nameof(IsFasePreparacion));
            OnPropertyChanged(nameof(IsFaseListo));
            OnPropertyChanged(nameof(IsFaseEntregado));
            OnPropertyChanged(nameof(MontoTotal));
        }

        private async Task ActualizarEstatus()
        {
            if (string.IsNullOrWhiteSpace(NuevoEstatus))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor selecciona un estatus.", "OK");
                return;
            }

            Pedido.Estatus = NuevoEstatus;
            var resultado = await _transaccionesService.ActualizarPedidoAsync(Pedido);

            if (resultado.Exito)
            {
                await Application.Current.MainPage.DisplayAlert("Fase Actualizada", $"La fase de producción del pedido se actualizó a: '{NuevoEstatus}'. Se envió notificación por correo.", "OK");
                OnPropertyChanged(nameof(Pedido));
                ActualizarPropiedadesFase();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar el estatus.\nDetalle: {resultado.Mensaje}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

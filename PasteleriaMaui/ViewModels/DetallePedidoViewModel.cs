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
            set { _pedido = value; OnPropertyChanged(); }
        }

        public string NuevoEstatus
        {
            get => _nuevoEstatus;
            set { _nuevoEstatus = value; OnPropertyChanged(); }
        }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";

        public ICommand VolverCommand { get; set; }
        public ICommand ActualizarEstatusCommand { get; set; }

        public DetallePedidoViewModel(Pedido pedidoSeleccionado, INavigation navigation)
        {
            _transaccionesService = new TransaccionesService();
            Pedido = pedidoSeleccionado;
            NuevoEstatus = Pedido.Estatus;

            VolverCommand = new Command(async () => await navigation.PopAsync());
            ActualizarEstatusCommand = new Command(async () => await ActualizarEstatus());
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
                await Application.Current.MainPage.DisplayAlert("Éxito", resultado.Mensaje, "OK");
                OnPropertyChanged(nameof(Pedido));
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

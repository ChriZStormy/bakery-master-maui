using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using PasteleriaMaui.Views;

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
        public bool IsCliente => AppSession.CurrentUser?.Rol == "Cliente";

        public ICommand VolverCommand { get; set; }
        public ICommand ActualizarEstatusCommand { get; set; }
        public ICommand PedirDeNuevoCommand { get; set; }

        public DetallePedidoViewModel(Pedido pedidoSeleccionado, INavigation navigation)
        {
            _transaccionesService = new TransaccionesService();
            Pedido = pedidoSeleccionado;
            NuevoEstatus = Pedido.Estatus;

            VolverCommand = new Command(async () => await navigation.PopAsync());
            ActualizarEstatusCommand = new Command(async () => await ActualizarEstatus());

            PedirDeNuevoCommand = new Command(async () =>
            {
                if (Pedido?.Pastel == null)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Info", "No se encontró la información del pastel para reordenar.", "OK");
                    }
                    return;
                }

                var navParam = new Dictionary<string, object>
                {
                    { "PastelAComprar", Pedido.Pastel }
                };

                await Shell.Current.GoToAsync(nameof(RegistrarPedidoPage), navParam);

                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("¡Pedido Repetido!", $"Se cargaron automáticamente los mismos ingredientes del pastel '{Pedido.Pastel.Nombre}' a tu orden actual.", "Aceptar");
                }
            });
        }

        private async Task ActualizarEstatus()
        {
            if (string.IsNullOrWhiteSpace(NuevoEstatus))
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor selecciona un estatus.", "OK");
                }
                return;
            }

            Pedido.Estatus = NuevoEstatus;
            var resultado = await _transaccionesService.ActualizarPedidoAsync(Pedido);

            if (resultado.Exito)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", resultado.Mensaje, "OK");
                }
                OnPropertyChanged(nameof(Pedido));
            }
            else
            {
                if (!AppSession.HasAuthError && Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar el estatus.\nDetalle: {resultado.Mensaje}", "OK");
                }
                AppSession.HasAuthError = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

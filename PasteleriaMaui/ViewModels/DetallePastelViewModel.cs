using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using PasteleriaMaui.Views;

namespace PasteleriaMaui.ViewModels
{
    [QueryProperty(nameof(PastelSeleccionado), "PastelSeleccionado")]
    public class DetallePastelViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _apiService;
        private Pastel _pastelSeleccionado;

        public Pastel PastelSeleccionado
        {
            get => _pastelSeleccionado;
            set
            {
                _pastelSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand RealizarPedidoCommand { get; }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";
        public bool IsCliente => AppSession.CurrentUser?.Rol == "Cliente";

        public DetallePastelViewModel()
        {
            _apiService = new PastelApiService();
            EditarCommand = new Command(async () => await EditarPastel());
            EliminarCommand = new Command(async () => await EliminarPastel());
            RealizarPedidoCommand = new Command(async () => await RealizarPedido());
        }

        private async Task EditarPastel()
        {
            if (AppSession.CurrentUser?.Rol == "Cliente") { await Application.Current.MainPage.DisplayAlert("Error", "Solo Administradores", "OK"); return; }
            if (PastelSeleccionado == null) return;
            var navigationParameter = new Dictionary<string, object>
            {
                { "PastelAEditar", PastelSeleccionado }
            };
            await Shell.Current.GoToAsync(nameof(EditarPastelPage), navigationParameter);
        }

        private async Task EliminarPastel()
        {
            if (AppSession.CurrentUser?.Rol == "Cliente") { await Application.Current.MainPage.DisplayAlert("Error", "Solo Administradores", "OK"); return; }
            if (PastelSeleccionado == null) return;

            bool confirmar = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas eliminar este pastel?", "Sí", "No");
            if (confirmar)
            {
                bool exito = await _apiService.EliminarPastelAsync(PastelSeleccionado.Id);
                if (exito)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Pastel eliminado.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el pastel.", "OK");
                }
            }
        }

        private async Task RealizarPedido()
        {
            if (PastelSeleccionado == null) return;
            if (AppSession.CurrentUser?.Rol == "Admin")
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Los administradores no pueden hacer pedidos.", "OK");
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "PastelAComprar", PastelSeleccionado }
            };
            await Shell.Current.GoToAsync(nameof(RegistrarPedidoPage), navigationParameter);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

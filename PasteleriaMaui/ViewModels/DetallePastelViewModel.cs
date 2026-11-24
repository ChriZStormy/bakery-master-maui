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
                OnPropertyChanged(nameof(BotonBajaTexto));
                OnPropertyChanged(nameof(BotonBajaBgColor));
                OnPropertyChanged(nameof(BotonBajaTextColor));
                OnPropertyChanged(nameof(EstadoPastelTexto));
                if (_pastelSeleccionado != null && _pastelSeleccionado.Categoria == null)
                {
                    _ = CargarDetallesCompletosAsync(_pastelSeleccionado.Id);
                }
            }
        }

        public string EstadoPastelTexto => PastelSeleccionado?.Activo == true ? (PastelSeleccionado?.Agotado == true ? "Agotado" : "Activo") : "Desactivado (Inactivo)";
        public string BotonBajaTexto => PastelSeleccionado?.Activo == true ? "Desactivar" : "Reactivar";
        public string BotonAgotadoTexto => PastelSeleccionado?.Agotado == true ? "Disponible" : "Marcar Agotado";
        public bool PuedeComprar => PastelSeleccionado?.Agotado != true;
        public Color BotonBajaBgColor => PastelSeleccionado?.Activo == true ? Color.FromArgb("#FFF0F0") : Color.FromArgb("#E8F5E9");
        public Color BotonBajaTextColor => PastelSeleccionado?.Activo == true ? Color.FromArgb("#D32F2F") : Color.FromArgb("#2E7D32");

        public List<string> PorcionesOpciones { get; } = new() { "10 personas", "20 personas", "30 personas" };
        private string _porcionesSeleccionadas = "10 personas";
        public string PorcionesSeleccionadas
        {
            get => _porcionesSeleccionadas;
            set
            {
                _porcionesSeleccionadas = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PrecioCalculado));
                OnPropertyChanged(nameof(PrecioCalculadoTexto));
            }
        }

        public decimal PrecioCalculado
        {
            get
            {
                var basePrice = PastelSeleccionado?.PrecioBase ?? 0m;
                if (basePrice <= 0) basePrice = 350m; // Valor base por defecto si aún no se asigna
                return PorcionesSeleccionadas switch
                {
                    "20 personas" => Math.Round(basePrice * 1.6m, 2),
                    "30 personas" => Math.Round(basePrice * 2.2m, 2),
                    _ => basePrice
                };
            }
        }

        public string PrecioCalculadoTexto => $"${PrecioCalculado:N2} MXN";

        private async Task CargarDetallesCompletosAsync(int id)
        {
            var completo = await _apiService.GetPastelByIdAsync(id);
            if (completo != null)
            {
                _pastelSeleccionado = completo;
                OnPropertyChanged(nameof(PastelSeleccionado));
                OnPropertyChanged(nameof(BotonBajaTexto));
                OnPropertyChanged(nameof(BotonBajaBgColor));
                OnPropertyChanged(nameof(BotonBajaTextColor));
                OnPropertyChanged(nameof(EstadoPastelTexto));
            }
        }

        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand ToggleAgotadoCommand { get; }
        public ICommand RealizarPedidoCommand { get; }
        public ICommand PersonalizarCommand { get; }
        public ICommand AgregarAlCarritoCommand { get; }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";
        public bool IsCliente => AppSession.CurrentUser?.Rol == "Cliente";

        public DetallePastelViewModel()
        {
            _apiService = new PastelApiService();
            EditarCommand = new Command(async () => await EditarPastel());
            EliminarCommand = new Command(async () => await EliminarPastel());
            ToggleAgotadoCommand = new Command(async () =>
            {
                if (PastelSeleccionado == null) return;
                PastelSeleccionado.Agotado = !PastelSeleccionado.Agotado;
                OnPropertyChanged(nameof(PastelSeleccionado));
                OnPropertyChanged(nameof(EstadoPastelTexto));
                OnPropertyChanged(nameof(BotonAgotadoTexto));
                OnPropertyChanged(nameof(PuedeComprar));
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Estado", PastelSeleccionado.Agotado ? "Pastel marcado como Agotado." : "Pastel marcado como Disponible.", "OK");
            });
            RealizarPedidoCommand = new Command(async () => await RealizarPedido());
            PersonalizarCommand = new Command(async () =>
            {
                if (PastelSeleccionado == null)
                {
                    await Shell.Current.GoToAsync(nameof(PedidoPersonalizadoPage));
                    return;
                }
                var navParam = new Dictionary<string, object> { { "PastelBase", PastelSeleccionado } };
                await Shell.Current.GoToAsync(nameof(PedidoPersonalizadoPage), navParam);
            });
            AgregarAlCarritoCommand = new Command(async () =>
            {
                if (PastelSeleccionado == null) return;
                CarritoService.Instance.Agregar(new CarritoItem
                {
                    Pastel = PastelSeleccionado,
                    Porciones = PorcionesSeleccionadas,
                    PrecioUnitario = PrecioCalculado,
                    Cantidad = 1
                });
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Carrito", $"{PastelSeleccionado.Nombre} ({PorcionesSeleccionadas}) agregado al carrito.", "OK");
            });
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

            string accion = PastelSeleccionado.Activo ? "dar de baja temporalmente" : "reactivar";
            bool confirmar = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Deseas {accion} este pastel?", "Sí", "No");
            if (confirmar)
            {
                bool exito;
                if (PastelSeleccionado.Activo)
                {
                    // Baja lógica
                    exito = await _apiService.EliminarPastelAsync(PastelSeleccionado.Id);
                }
                else
                {
                    // Reactivación
                    exito = await _apiService.ReactivarPastelAsync(PastelSeleccionado.Id);
                }

                if (exito)
                {
                    PastelSeleccionado.Activo = !PastelSeleccionado.Activo;
                    string mensajeExito = PastelSeleccionado.Activo ? "Pastel reactivado exitosamente." : "Pastel dado de baja exitosamente (baja lógica).";
                    await Application.Current.MainPage.DisplayAlert("Éxito", mensajeExito, "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    if (!AppSession.HasAuthError) { await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el estado del pastel.", "OK"); } AppSession.HasAuthError = false;
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

            if (!PastelSeleccionado.Activo)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Este pastel está temporalmente no disponible.", "OK");
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "PastelAComprar", PastelSeleccionado }
            };
            await Shell.Current.GoToAsync(nameof(RegistrarPedidoPage), navigationParameter);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

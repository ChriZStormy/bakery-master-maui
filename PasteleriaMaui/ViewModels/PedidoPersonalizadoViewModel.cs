using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    public class PedidoPersonalizadoViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _pastelService;
        private readonly TransaccionesService _transaccionesService;

        public ObservableCollection<CatBizcocho> Bizcochos { get; set; } = new();
        public ObservableCollection<CatRelleno> Rellenos { get; set; } = new();
        public ObservableCollection<CatGlaseado> Glaseados { get; set; } = new();
        public ObservableCollection<CatCategoria> Categorias { get; set; } = new();

        public string Nombre { get; set; }
        public string Tamanio { get; set; }
        public string FotoUrl { get; set; }
        public DateTime FechaEntrega { get; set; } = DateTime.Today.AddDays(2);
        public DateTime FechaMinima => DateTime.Today;
        public string Cantidad { get; set; } = "1";

        public CatBizcocho BizcochoSeleccionado { get; set; }
        public CatRelleno RellenoSeleccionado { get; set; }
        public CatGlaseado GlaseadoSeleccionado { get; set; }
        public CatCategoria CategoriaSeleccionada { get; set; }

        public ICommand GuardarPedidoCommand { get; }

        public PedidoPersonalizadoViewModel()
        {
            _pastelService = new PastelApiService();
            _transaccionesService = new TransaccionesService();
            GuardarPedidoCommand = new Command(async () => await GuardarPedido());
            CargarCatalogos();
        }

        private async void CargarCatalogos()
        {
            var bizcos = await _pastelService.GetBizcochosAsync();
            foreach (var b in bizcos) Bizcochos.Add(b);

            var rels = await _pastelService.GetRellenosAsync();
            foreach (var r in rels) Rellenos.Add(r);

            var glas = await _pastelService.GetGlaseadosAsync();
            foreach (var g in glas) Glaseados.Add(g);

            var cats = await _pastelService.GetCategoriasAsync();
            foreach (var c in cats) Categorias.Add(c);
        }

        private async Task GuardarPedido()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || BizcochoSeleccionado == null || RellenoSeleccionado == null || GlaseadoSeleccionado == null || CategoriaSeleccionada == null || string.IsNullOrWhiteSpace(Tamanio) || string.IsNullOrWhiteSpace(Cantidad))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            int qty;
            if (!int.TryParse(Cantidad, out qty) || qty <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La cantidad debe ser un número válido mayor a 0.", "OK");
                return;
            }

            var nuevoPastel = new Pastel
            {
                Nombre = Nombre,
                Tamanio = Tamanio,
                FotoUrl = FotoUrl,
                BizcochoId = BizcochoSeleccionado.Id,
                RellenoId = RellenoSeleccionado.Id,
                GlaseadoId = GlaseadoSeleccionado.Id,
                CategoriaId = CategoriaSeleccionada.Id,
                IsPersonalizado = true
            };

            var pastelCreado = await _pastelService.RegistrarPastelAsync(nuevoPastel);
            if (pastelCreado != null)
            {
                var nuevoPedido = new Pedido
                {
                    PastelId = pastelCreado.Id,
                    UsuarioId = AppSession.CurrentUser.Id,
                    Cantidad = qty,
                    FechaEntrega = FechaEntrega
                };

                var exito = await _transaccionesService.CrearPedidoAsync(nuevoPedido);
                if (exito)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Tu pedido personalizado ha sido creado.", "OK");
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El pastel se creó, pero falló el pedido.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar tu pastel.", "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using PasteleriaMaui.Views;

namespace PasteleriaMaui.ViewModels
{
    public class ListaPastelesViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _apiService;
        private List<Pastel> _todosLosPasteles = new();
        private ObservableCollection<Pastel> _pasteles;
        private ObservableCollection<CatCategoria> _categorias = new();
        private CatCategoria? _categoriaSeleccionada;
        private string _searchText = string.Empty;

        public ObservableCollection<Pastel> Pasteles
        {
            get => _pasteles;
            set { _pasteles = value; OnPropertyChanged(); OnPropertyChanged(nameof(MostrarMensajeVacio)); }
        }

        public ObservableCollection<CatCategoria> Categorias
        {
            get => _categorias;
            set { _categorias = value; OnPropertyChanged(); }
        }

        public CatCategoria? CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged();
                AplicarFiltros();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                AplicarFiltros();
            }
        }

        public bool MostrarMensajeVacio => Pasteles == null || Pasteles.Count == 0;

        public ICommand CargarPastelesCommand { get; }
        public ICommand VerDetalleCommand { get; }
        public ICommand RegistrarNuevoCommand { get; }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";
        public bool IsCliente => AppSession.CurrentUser?.Rol == "Cliente";
        public string BotonAccionTexto => IsAdmin ? "Detalles" : "Pedir ya";

        public ListaPastelesViewModel()
        {
            _apiService = new PastelApiService();
            Pasteles = new ObservableCollection<Pastel>();
            
            CargarPastelesCommand = new Command(async () => await CargarPasteles());
            VerDetalleCommand = new Command<Pastel>(async (v) => await VerDetalle(v));
            RegistrarNuevoCommand = new Command(async () => await RegistrarNuevo());
        }

        public async Task CargarPasteles()
        {
            // Cargar categorías si aún no están cargadas
            if (Categorias.Count == 0)
            {
                var cats = await _apiService.GetCategoriasAsync();
                Categorias.Clear();
                Categorias.Add(new CatCategoria { Id = 0, categoria = "Todas" });
                foreach (var c in cats) Categorias.Add(c);
                _categoriaSeleccionada = Categorias.FirstOrDefault();
                OnPropertyChanged(nameof(CategoriaSeleccionada));
            }

            // Filtro por rol: Si es cliente, solo solicitar y cargar pasteles activos
            bool soloActivos = !IsAdmin;
            var pastelesApi = await _apiService.GetPastelesAsync(soloActivos);
            _todosLosPasteles = pastelesApi.Where(v => IsAdmin || v.Activo).ToList();
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            var filtrados = _todosLosPasteles.AsEnumerable();
            if (CategoriaSeleccionada != null && CategoriaSeleccionada.Id != 0)
            {
                filtrados = filtrados.Where(p => p.CategoriaId == CategoriaSeleccionada.Id);
            }
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtrados = filtrados.Where(p => p.Nombre.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            Pasteles.Clear();
            foreach (var v in filtrados)
            {
                Pasteles.Add(v);
            }
            OnPropertyChanged(nameof(MostrarMensajeVacio));
        }

        private async Task RegistrarNuevo()
        {
            await Shell.Current.GoToAsync(nameof(RegistrarPastelPage));
        }

        private async Task VerDetalle(Pastel? pastel)
        {
            if (pastel == null) return;
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "PastelSeleccionado", pastel }
            };
            await Shell.Current.GoToAsync(nameof(DetallePastelPage), navigationParameter);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

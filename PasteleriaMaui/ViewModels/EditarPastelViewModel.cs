using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using System.Collections.ObjectModel;

namespace PasteleriaMaui.ViewModels
{
    [QueryProperty(nameof(PastelAEditar), "PastelAEditar")]
    public class EditarPastelViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _apiService;
        private Pastel _pastelAEditar;

        private string _nombre;
        private string _tamanio;
        private string _fotoUrl;

        public ObservableCollection<CatBizcocho> Bizcochos { get; set; } = new();
        public ObservableCollection<CatRelleno> Rellenos { get; set; } = new();
        public ObservableCollection<CatGlaseado> Glaseados { get; set; } = new();
        public ObservableCollection<CatCategoria> Categorias { get; set; } = new();

        private CatBizcocho _bizcochoSeleccionado;
        public CatBizcocho BizcochoSeleccionado
        {
            get => _bizcochoSeleccionado;
            set { _bizcochoSeleccionado = value; OnPropertyChanged(); }
        }

        private CatRelleno _rellenoSeleccionado;
        public CatRelleno RellenoSeleccionado
        {
            get => _rellenoSeleccionado;
            set { _rellenoSeleccionado = value; OnPropertyChanged(); }
        }

        private CatGlaseado _glaseadoSeleccionado;
        public CatGlaseado GlaseadoSeleccionado
        {
            get => _glaseadoSeleccionado;
            set { _glaseadoSeleccionado = value; OnPropertyChanged(); }
        }

        private CatCategoria _categoriaSeleccionada;
        public CatCategoria CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; OnPropertyChanged(); }
        }

        public Pastel PastelAEditar
        {
            get => _pastelAEditar;
            set
            {
                _pastelAEditar = value;
                if (_pastelAEditar != null)
                {
                    Nombre = _pastelAEditar.Nombre;
                    Tamanio = _pastelAEditar.Tamanio;
                    FotoUrl = _pastelAEditar.FotoUrl;
                    CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.Id == _pastelAEditar.CategoriaId);
                    BizcochoSeleccionado = Bizcochos.FirstOrDefault(b => b.Id == _pastelAEditar.BizcochoId);
                    RellenoSeleccionado = Rellenos.FirstOrDefault(r => r.Id == _pastelAEditar.RellenoId);
                    GlaseadoSeleccionado = Glaseados.FirstOrDefault(g => g.Id == _pastelAEditar.GlaseadoId);
                }
                OnPropertyChanged();
            }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }
        public string FotoUrl
        {
            get => _fotoUrl;
            set { _fotoUrl = value; OnPropertyChanged(); }
        }
        public string Tamanio
        {
            get => _tamanio;
            set { _tamanio = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> TamanioList { get; } = new ObservableCollection<string>
        {
            "Chico", "Mediano", "Grande", "Extra Grande"
        };

        public ICommand GuardarCommand { get; }

        public EditarPastelViewModel()
        {
            _apiService = new PastelApiService();
            GuardarCommand = new Command(async () => await GuardarPastel());
            CargarCatalogos();
        }

        private async void CargarCatalogos()
        {
            var bizcos = await _apiService.GetBizcochosAsync();
            foreach (var b in bizcos) Bizcochos.Add(b);

            var rels = await _apiService.GetRellenosAsync();
            foreach (var r in rels) Rellenos.Add(r);

            var glas = await _apiService.GetGlaseadosAsync();
            foreach (var g in glas) Glaseados.Add(g);

            var cats = await _apiService.GetCategoriasAsync();
            foreach (var c in cats) Categorias.Add(c);
            
            // Re-evaluar selección después de cargar catálogos
            if (_pastelAEditar != null)
            {
                CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.Id == _pastelAEditar.CategoriaId);
                BizcochoSeleccionado = Bizcochos.FirstOrDefault(b => b.Id == _pastelAEditar.BizcochoId);
                RellenoSeleccionado = Rellenos.FirstOrDefault(r => r.Id == _pastelAEditar.RellenoId);
                GlaseadoSeleccionado = Glaseados.FirstOrDefault(g => g.Id == _pastelAEditar.GlaseadoId);
            }
        }

        private async Task GuardarPastel()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Tamanio) || BizcochoSeleccionado == null || RellenoSeleccionado == null || GlaseadoSeleccionado == null || CategoriaSeleccionada == null)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Por favor llena todos los campos.", "OK");
                return;
            }

            var pastelEditado = new Pastel
            {
                Id = PastelAEditar.Id,
                Nombre = this.Nombre,
                Tamanio = this.Tamanio,
                FotoUrl = this.FotoUrl,
                CategoriaId = this.CategoriaSeleccionada.Id,
                BizcochoId = this.BizcochoSeleccionado.Id,
                RellenoId = this.RellenoSeleccionado.Id,
                GlaseadoId = this.GlaseadoSeleccionado.Id
            };

            bool exito = await _apiService.ActualizarPastelAsync(pastelEditado);

            if (exito)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Pastel actualizado.", "OK");
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el pastel.", "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

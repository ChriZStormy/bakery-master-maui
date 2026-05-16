using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using System.Collections.ObjectModel;

namespace PasteleriaMaui.ViewModels
{
	public class RegistrarPastelViewModel : INotifyPropertyChanged
	{
		private readonly PastelApiService _apiService;

		private string _nombre = "";
		private string _tamanio = "";
		private string _fotoUrl = "";

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

        public ObservableCollection<CatBizcocho> Bizcochos { get; set; } = new();
        public ObservableCollection<CatRelleno> Rellenos { get; set; } = new();
        public ObservableCollection<CatGlaseado> Glaseados { get; set; } = new();
        public ObservableCollection<CatCategoria> Categorias { get; set; } = new();

        public CatBizcocho BizcochoSeleccionado { get; set; }
        public CatRelleno RellenoSeleccionado { get; set; }
        public CatGlaseado GlaseadoSeleccionado { get; set; }
        public CatCategoria CategoriaSeleccionada { get; set; }

		public ICommand GuardarCommand { get; }

		public RegistrarPastelViewModel()
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
        }

		private async Task GuardarPastel()
		{
			// Validaciones
			if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Tamanio) || BizcochoSeleccionado == null || RellenoSeleccionado == null || GlaseadoSeleccionado == null || CategoriaSeleccionada == null)
			{
				await Application.Current.MainPage.DisplayAlert("Aviso", "Por favor llena todos los campos.", "OK");
				return;
			}

			var pastel = new Pastel
			{
				Nombre = this.Nombre,
				Tamanio = this.Tamanio,
                FotoUrl = this.FotoUrl,
				CategoriaId = this.CategoriaSeleccionada.Id,
                BizcochoId = this.BizcochoSeleccionado.Id,
                RellenoId = this.RellenoSeleccionado.Id,
                GlaseadoId = this.GlaseadoSeleccionado.Id
			};

			var result = await _apiService.RegistrarPastelAsync(pastel);

			if (result != null)
			{
				await Application.Current.MainPage.DisplayAlert("Éxito", "Pastel registrado en la base de datos.", "OK");
				await Shell.Current.GoToAsync(".."); // Regresar al inicio
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "No se pudo conectar con la API.", "OK");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
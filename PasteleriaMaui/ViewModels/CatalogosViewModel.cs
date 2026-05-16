using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using Microsoft.Maui.Controls;

namespace PasteleriaMaui.ViewModels
{
    public class CatalogoItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public object OriginalObject { get; set; }
    }

    public class CatalogosViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _apiService;
        public ObservableCollection<CatalogoItem> Elementos { get; set; }

        private string _tabActual = "Categorías";
        public string TabActual
        {
            get => _tabActual;
            set { _tabActual = value; OnPropertyChanged(); }
        }
        
        public ICommand SeleccionarTabCommand { get; }
        public ICommand AgregarElementoCommand { get; }
        public ICommand EliminarElementoCommand { get; }

        public CatalogosViewModel()
        {
            _apiService = new PastelApiService();
            Elementos = new ObservableCollection<CatalogoItem>();
            SeleccionarTabCommand = new Command<string>(async (t) => await SeleccionarTab(t));
            AgregarElementoCommand = new Command(async () => await AgregarElemento());
            EliminarElementoCommand = new Command<CatalogoItem>(async (c) => await EliminarElemento(c));
            
            // Load initially
            _ = CargarDatos();
        }

        private async Task SeleccionarTab(string tab)
        {
            TabActual = tab;
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            Elementos.Clear();
            if (TabActual == "Categorías")
            {
                var cats = await _apiService.GetCategoriasAsync();
                foreach (var c in cats) Elementos.Add(new CatalogoItem { Id = c.Id, DisplayName = c.categoria, OriginalObject = c });
            }
            else if (TabActual == "Bizcochos")
            {
                var bizcos = await _apiService.GetBizcochosAsync();
                foreach (var b in bizcos) Elementos.Add(new CatalogoItem { Id = b.Id, DisplayName = b.Nombre, OriginalObject = b });
            }
            else if (TabActual == "Rellenos")
            {
                var rels = await _apiService.GetRellenosAsync();
                foreach (var r in rels) Elementos.Add(new CatalogoItem { Id = r.Id, DisplayName = r.Nombre, OriginalObject = r });
            }
            else if (TabActual == "Glaseados")
            {
                var glas = await _apiService.GetGlaseadosAsync();
                foreach (var g in glas) Elementos.Add(new CatalogoItem { Id = g.Id, DisplayName = g.Nombre, OriginalObject = g });
            }
        }

        private async Task AgregarElemento()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync($"Nuevo(a) {TabActual}", "Escribe el nombre:");
            if (!string.IsNullOrWhiteSpace(result))
            {
                if (TabActual == "Categorías")
                    await _apiService.RegistrarCategoriaAsync(new CatCategoria { categoria = result });
                else if (TabActual == "Bizcochos")
                    await _apiService.RegistrarBizcochoAsync(new CatBizcocho { Nombre = result });
                else if (TabActual == "Rellenos")
                    await _apiService.RegistrarRellenoAsync(new CatRelleno { Nombre = result });
                else if (TabActual == "Glaseados")
                    await _apiService.RegistrarGlaseadoAsync(new CatGlaseado { Nombre = result });
                
                await CargarDatos();
            }
        }

        private async Task EliminarElemento(CatalogoItem c)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Eliminar {c.DisplayName}?", "Sí", "No");
            if(confirm)
            {
                if (TabActual == "Categorías")
                    await _apiService.EliminarCategoriaAsync(c.Id);
                else if (TabActual == "Bizcochos")
                    await _apiService.EliminarBizcochoAsync(c.Id);
                else if (TabActual == "Rellenos")
                    await _apiService.EliminarRellenoAsync(c.Id);
                else if (TabActual == "Glaseados")
                    await _apiService.EliminarGlaseadoAsync(c.Id);

                await CargarDatos();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

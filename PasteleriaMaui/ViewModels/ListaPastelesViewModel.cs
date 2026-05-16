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
        private ObservableCollection<Pastel> _pasteles;

        public ObservableCollection<Pastel> Pasteles
        {
            get => _pasteles;
            set { _pasteles = value; OnPropertyChanged(); }
        }

        public ICommand CargarPastelesCommand { get; }
        public ICommand VerDetalleCommand { get; }
        public ICommand RegistrarNuevoCommand { get; }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";

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
            var pastelesApi = await _apiService.GetPastelesAsync();
            Pasteles.Clear();
            foreach (var v in pastelesApi)
            {
                Pasteles.Add(v);
            }
        }

        private async Task RegistrarNuevo()
        {
            await Shell.Current.GoToAsync(nameof(RegistrarPastelPage));
        }

        private async Task VerDetalle(Pastel pastel)
        {
            if (pastel == null) return;
            
            var navigationParameter = new Dictionary<string, object>
            {
                { "PastelSeleccionado", pastel }
            };
            await Shell.Current.GoToAsync(nameof(DetallePastelPage), navigationParameter);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

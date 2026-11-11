using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class GestionUsuariosPage : ContentPage
    {
        private readonly GestionUsuariosViewModel _viewModel;

        public GestionUsuariosPage(GestionUsuariosViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        public GestionUsuariosPage() : this(IPlatformApplication.Current?.Services.GetService<GestionUsuariosViewModel>() ?? new GestionUsuariosViewModel())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarUsuariosAsync();
        }
    }
}

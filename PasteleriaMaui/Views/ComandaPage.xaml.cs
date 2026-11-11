using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class ComandaPage : ContentPage
    {
        private readonly ComandaViewModel _viewModel;

        public ComandaPage(ComandaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // Constructor sin parámetros para el enrutador de Shell si no usa DI directamente
        public ComandaPage() : this(new ComandaViewModel())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InicializarAsync();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.DisposeAsync();
        }
    }
}

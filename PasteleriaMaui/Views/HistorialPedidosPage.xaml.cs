using Microsoft.Maui.Controls;
using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class HistorialPedidosPage : ContentPage
    {
        private readonly HistorialPedidosViewModel _viewModel;

        public HistorialPedidosPage(HistorialPedidosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        public HistorialPedidosPage() : this(new HistorialPedidosViewModel())
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InicializarAsync();
        }
    }
}

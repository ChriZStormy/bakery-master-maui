using Microsoft.Maui.Controls;
using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class PedidoPersonalizadoPage : ContentPage
    {
        public PedidoPersonalizadoPage() : this(new PedidoPersonalizadoViewModel())
        {
        }

        public PedidoPersonalizadoPage(PedidoPersonalizadoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PedidoPersonalizadoViewModel vm)
            {
                await vm.CargarCatalogosAsync();
            }
        }
    }
}

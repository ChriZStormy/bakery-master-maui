using Microsoft.Maui.Controls;
using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class RegistrarPedidoPage : ContentPage
    {
        public RegistrarPedidoPage(RegistrarPedidoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public RegistrarPedidoPage() : this(new RegistrarPedidoViewModel())
        {
        }
    }
}

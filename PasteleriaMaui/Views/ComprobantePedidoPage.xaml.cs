using Microsoft.Maui.Controls;
using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views
{
    public partial class ComprobantePedidoPage : ContentPage
    {
        public ComprobantePedidoPage(ComprobantePedidoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ComprobantePedidoPage() : this(new ComprobantePedidoViewModel())
        {
        }
    }
}

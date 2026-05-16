using Microsoft.Maui.Controls;

namespace PasteleriaMaui.Views
{
    public partial class PedidoPersonalizadoPage : ContentPage
    {
        public PedidoPersonalizadoPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.PedidoPersonalizadoViewModel();
        }
    }
}

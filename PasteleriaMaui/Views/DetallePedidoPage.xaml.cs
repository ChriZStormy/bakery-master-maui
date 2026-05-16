using PasteleriaMaui.Models;
using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views;

public partial class DetallePedidoPage : ContentPage
{
	public DetallePedidoPage(Pedido pedidoSeleccionado)
	{
		InitializeComponent();
        BindingContext = new DetallePedidoViewModel(pedidoSeleccionado, this.Navigation);
	}
}

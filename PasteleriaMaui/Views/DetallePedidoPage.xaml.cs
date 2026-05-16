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

    private async void OnExpandImageTapped(object sender, TappedEventArgs e)
    {
        var url = e.Parameter as string;
        if (!string.IsNullOrWhiteSpace(url))
        {
            await Navigation.PushModalAsync(new ImagenLightboxPage(url), true);
        }
    }
}

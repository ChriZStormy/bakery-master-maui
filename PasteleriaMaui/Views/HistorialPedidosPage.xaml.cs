using System.Collections.ObjectModel;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

public partial class HistorialPedidosPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;

    public ObservableCollection<Pedido> Pedidos { get; set; } = new ObservableCollection<Pedido>();

	public HistorialPedidosPage()
	{
		InitializeComponent();
        _transaccionesService = new TransaccionesService();
        BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        int? filterId = null;
        if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
        {
            filterId = AppSession.CurrentUser.Id;
            BtnNuevoPedido.IsVisible = true;
            BtnCalendario.IsVisible = false;
            LblLeyenda.IsVisible = true;
            this.Title = "Realizar pedido";
        }
        else 
        {
            BtnNuevoPedido.IsVisible = false;
            BtnCalendario.IsVisible = true;
            LblLeyenda.IsVisible = false;
            this.Title = "Historial de Pedidos";
        }

        var pedidos = await _transaccionesService.GetPedidosAsync(filterId);
        
        Pedidos.Clear();
        foreach (var p in pedidos)
        {
            Pedidos.Add(p);
        }
    }

    private async void OnRealizarPedidoPersonalizadoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PedidoPersonalizadoPage));
    }

    private async void OnVerCalendarioClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CalendarioPedidosPage));
    }

    private async void OnPedidoSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Pedido pedidoSeleccionado)
        {
            await Shell.Current.Navigation.PushAsync(new DetallePedidoPage(pedidoSeleccionado));
            // Deselect to allow clicking again
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
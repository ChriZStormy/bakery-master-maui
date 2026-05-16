using System.Collections.ObjectModel;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

public partial class HistorialPedidosPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;
    private List<Pedido> _todosLosPedidos = new();
    private string _currentMainTab = "En Proceso";
    private string _currentSubTab = "Pasteles";

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
        
        _todosLosPedidos = pedidos.ToList();
        
        if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Admin")
        {
            AdminTabsContainer.IsVisible = true;
            FiltrarPedidosAdmin();
        }
        else
        {
            AdminTabsContainer.IsVisible = false;
            Pedidos.Clear();
            foreach (var p in _todosLosPedidos)
            {
                Pedidos.Add(p);
            }
        }
    }

    private void FiltrarPedidosAdmin()
    {
        var filtrados = _todosLosPedidos.AsEnumerable();

        // Main Tab Filter
        if (_currentMainTab == "En Proceso")
        {
            filtrados = filtrados.Where(p => p.Estatus == "Pendiente" || p.Estatus == "En Proceso");
        }
        else if (_currentMainTab == "Entregados")
        {
            filtrados = filtrados.Where(p => p.Estatus == "Entregado" || p.Estatus == "Entregados");
        }
        else if (_currentMainTab == "Cancelados")
        {
            filtrados = filtrados.Where(p => p.Estatus == "Cancelado" || p.Estatus == "Cancelados");
        }

        // Sub Tab Filter
        if (_currentSubTab == "Pasteles")
        {
            filtrados = filtrados.Where(p => p.Pastel != null);
        }
        else if (_currentSubTab == "Otros")
        {
            filtrados = Enumerable.Empty<Pedido>(); // No hay otros productos de momento
        }

        var filtradosOrdenados = filtrados.OrderByDescending(p => p.Fecha);

        Pedidos.Clear();
        foreach (var p in filtradosOrdenados)
        {
            Pedidos.Add(p);
        }

        ActualizarEstiloTabs();
    }

    private void ActualizarEstiloTabs()
    {
        TabEnProceso.BackgroundColor = _currentMainTab == "En Proceso" ? Color.FromArgb("#8B4513") : Color.FromArgb("#DEB887");
        TabEnProceso.TextColor = _currentMainTab == "En Proceso" ? Colors.White : Colors.Black;

        TabEntregados.BackgroundColor = _currentMainTab == "Entregados" ? Color.FromArgb("#8B4513") : Color.FromArgb("#DEB887");
        TabEntregados.TextColor = _currentMainTab == "Entregados" ? Colors.White : Colors.Black;

        TabCancelados.BackgroundColor = _currentMainTab == "Cancelados" ? Color.FromArgb("#8B4513") : Color.FromArgb("#DEB887");
        TabCancelados.TextColor = _currentMainTab == "Cancelados" ? Colors.White : Colors.Black;

        SubTabPasteles.BackgroundColor = _currentSubTab == "Pasteles" ? Color.FromArgb("#5C4033") : Color.FromArgb("#D2B48C");
        SubTabPasteles.TextColor = _currentSubTab == "Pasteles" ? Colors.White : Colors.Black;

        SubTabOtros.BackgroundColor = _currentSubTab == "Otros" ? Color.FromArgb("#5C4033") : Color.FromArgb("#D2B48C");
        SubTabOtros.TextColor = _currentSubTab == "Otros" ? Colors.White : Colors.Black;
    }

    private void OnMainTabClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            _currentMainTab = btn.Text;
            FiltrarPedidosAdmin();
        }
    }

    private void OnSubTabClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            _currentSubTab = btn.Text;
            FiltrarPedidosAdmin();
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
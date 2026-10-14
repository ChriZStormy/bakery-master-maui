using System.Collections.ObjectModel;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

public partial class HistorialPedidosPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;
    private List<Pedido> _todosLosPedidos = new();
    private string _currentTab = "Pendientes / Cocina";

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
        await CargarPedidos();
    }

    private async Task CargarPedidos()
    {
        var rol = AppSession.CurrentUser?.Rol;
        int? filterId = null;

        if (rol == "Cliente")
        {
            filterId = AppSession.CurrentUser?.Id;
            BtnNuevoPedido.IsVisible = true;
            BtnCalendario.IsVisible = false;
            LblLeyenda.IsVisible = true;
            LblLeyenda.Text = "Pedidos que has realizado";
            this.Title = "Mis Pedidos";
            TabsContainer.IsVisible = false;
        }
        else if (rol == "Repostero" || rol == "Empleado/Repostero")
        {
            BtnNuevoPedido.IsVisible = false;
            BtnCalendario.IsVisible = true;
            LblLeyenda.IsVisible = true;
            LblLeyenda.Text = "Taller de Repostería - Comanda de Cocina";
            this.Title = "Comanda de Cocina";
            TabsContainer.IsVisible = true;
        }
        else // Admin
        {
            BtnNuevoPedido.IsVisible = false;
            BtnCalendario.IsVisible = true;
            LblLeyenda.IsVisible = true;
            LblLeyenda.Text = "Gestión de Pedidos";
            this.Title = "Historial de Pedidos";
            TabsContainer.IsVisible = true;
        }

        var pedidos = await _transaccionesService.GetPedidosAsync(filterId);
        _todosLosPedidos = pedidos?.ToList() ?? new List<Pedido>();

        if (rol == "Cliente")
        {
            // Para cliente: ordenados con los más recientes primero
            Pedidos.Clear();
            foreach (var p in _todosLosPedidos.OrderByDescending(p => p.Fecha))
            {
                Pedidos.Add(p);
            }
        }
        else
        {
            FiltrarYOrdenarPedidos();
        }
    }

    private void FiltrarYOrdenarPedidos()
    {
        var filtrados = _todosLosPedidos.AsEnumerable();

        if (_currentTab == "Pendientes / Cocina" || _currentTab == "En Proceso")
        {
            // HU-07 Escenario 1: Pedidos pendientes y en elaboración ordenados cronológicamente por hora de entrega
            filtrados = filtrados
                .Where(p => p.Estatus == "Pendiente" || p.Estatus == "En Preparación" || p.Estatus == "En Proceso")
                .OrderBy(p => p.FechaEntrega); // Orden cronológico por hora de entrega requerida
        }
        else if (_currentTab == "Listos para entrega")
        {
            filtrados = filtrados
                .Where(p => p.Estatus == "Listo para entrega")
                .OrderBy(p => p.FechaEntrega);
        }
        else if (_currentTab == "Entregados")
        {
            filtrados = filtrados
                .Where(p => p.Estatus == "Entregado" || p.Estatus == "Entregados")
                .OrderByDescending(p => p.Fecha);
        }
        else if (_currentTab == "Cancelados")
        {
            filtrados = filtrados
                .Where(p => p.Estatus == "Cancelado" || p.Estatus == "Cancelados")
                .OrderByDescending(p => p.Fecha);
        }
        else // "Todos"
        {
            filtrados = filtrados.OrderBy(p => p.FechaEntrega);
        }

        Pedidos.Clear();
        foreach (var p in filtrados)
        {
            Pedidos.Add(p);
        }

        ActualizarEstiloTabs();
    }

    private void ActualizarEstiloTabs()
    {
        ActualizarBotonTab(TabEnProceso, _currentTab == "Pendientes / Cocina" || _currentTab == "En Proceso");
        ActualizarBotonTab(TabListos, _currentTab == "Listos para entrega");
        ActualizarBotonTab(TabEntregados, _currentTab == "Entregados");
        ActualizarBotonTab(TabCancelados, _currentTab == "Cancelados");
        ActualizarBotonTab(TabTodos, _currentTab == "Todos");
    }

    private void ActualizarBotonTab(Button btn, bool activo)
    {
        if (btn == null) return;
        btn.BackgroundColor = activo ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        btn.TextColor = activo ? Colors.White : Color.FromArgb("#795548");
    }

    private void OnMainTabClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            _currentTab = btn.Text;
            FiltrarYOrdenarPedidos();
        }
    }

    private async void OnIniciarElaboracionRapidaClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Pedido pedido)
        {
            bool confirmar = await DisplayAlert("Taller de Repostería", $"¿Iniciar elaboración del pedido #{pedido.Id} ({pedido.Pastel?.Nombre})?", "Iniciar", "Cancelar");
            if (!confirmar) return;

            var resultado = await _transaccionesService.ActualizarEstatusPedidoAsync(pedido.Id, "En Preparación");
            if (resultado.Exito)
            {
                await DisplayAlert("Fase de Producción Actualizada", $"El pedido #{pedido.Id} pasó a 'En Preparación'. Se envió el correo de notificación al cliente.", "OK");
                await CargarPedidos();
            }
            else
            {
                await DisplayAlert("Error", resultado.Mensaje, "OK");
            }
        }
    }

    private async void OnFinalizarMontajeRapidoClicked(object sender, EventArgs e)
    {
        if ((sender as Button)?.BindingContext is Pedido pedido)
        {
            bool confirmar = await DisplayAlert("Taller de Repostería", $"¿Finalizar montaje y marcar como 'Listo para entrega' el pedido #{pedido.Id}?", "Finalizar", "Cancelar");
            if (!confirmar) return;

            var resultado = await _transaccionesService.ActualizarEstatusPedidoAsync(pedido.Id, "Listo para entrega");
            if (resultado.Exito)
            {
                await DisplayAlert("Pedido Terminado", $"El pedido #{pedido.Id} está 'Listo para entrega'. Se disparó el correo notificando al cliente que su producto ya está disponible para recolección o va en ruta.", "OK");
                await CargarPedidos();
            }
            else
            {
                await DisplayAlert("Error", resultado.Mensaje, "OK");
            }
        }
    }

    private async void OnRefrescarViewRefreshing(object sender, EventArgs e)
    {
        await CargarPedidos();
        RefrescarView.IsRefreshing = false;
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
            if (sender is CollectionView cv)
            {
                cv.SelectedItem = null;
            }
        }
    }
}
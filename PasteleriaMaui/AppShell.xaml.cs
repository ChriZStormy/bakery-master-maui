using PasteleriaMaui.Views;

namespace PasteleriaMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
			Routing.RegisterRoute(nameof(RegistrarPastelPage), typeof(RegistrarPastelPage));
			Routing.RegisterRoute(nameof(ListaPastelesPage), typeof(ListaPastelesPage));
			Routing.RegisterRoute(nameof(DetallePastelPage), typeof(DetallePastelPage));
			Routing.RegisterRoute(nameof(EditarPastelPage), typeof(EditarPastelPage));
			Routing.RegisterRoute(nameof(HistorialPedidosPage), typeof(HistorialPedidosPage));
			Routing.RegisterRoute(nameof(ReporteMermaPage), typeof(ReporteMermaPage));
			Routing.RegisterRoute(nameof(CatalogosPage), typeof(CatalogosPage));
			Routing.RegisterRoute(nameof(PedidoPersonalizadoPage), typeof(PedidoPersonalizadoPage));
            Routing.RegisterRoute(nameof(RegistrarPedidoPage), typeof(RegistrarPedidoPage));
            Routing.RegisterRoute(nameof(ComprobantePedidoPage), typeof(ComprobantePedidoPage));
            Routing.RegisterRoute(nameof(CalendarioPedidosPage), typeof(CalendarioPedidosPage));
            Routing.RegisterRoute(nameof(GestionUsuariosPage), typeof(GestionUsuariosPage));
            Routing.RegisterRoute(nameof(ComandaPage), typeof(ComandaPage));
            Routing.RegisterRoute(nameof(ReporteEntregasPage), typeof(ReporteEntregasPage));
            Routing.RegisterRoute(nameof(DetallePedidoPage), typeof(DetallePedidoPage));
            Routing.RegisterRoute(nameof(CarritoPage), typeof(CarritoPage));
		}

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            var target = args.Target?.Location?.OriginalString ?? string.Empty;
            var rol = AppSession.CurrentUser?.Rol ?? string.Empty;

            if (string.IsNullOrEmpty(rol)) return;

            bool isAdminRoute = target.Contains(nameof(RegistrarPastelPage)) ||
                                target.Contains(nameof(EditarPastelPage)) ||
                                target.Contains(nameof(CatalogosPage)) ||
                                target.Contains(nameof(GestionUsuariosPage)) ||
                                target.Contains(nameof(ReporteEntregasPage));

            bool isKitchenRoute = target.Contains(nameof(ComandaPage));

            if (rol == "Cliente" && (isAdminRoute || isKitchenRoute))
            {
                args.Cancel();
                if (CurrentPage != null)
                    await CurrentPage.DisplayAlert("Acceso Denegado", "No tienes permisos para acceder a esta sección.", "OK");
            }
            else if ((rol == "Repostero" || rol == "Empleado") && isAdminRoute)
            {
                args.Cancel();
                if (CurrentPage != null)
                    await CurrentPage.DisplayAlert("Acceso Denegado", "Solo el Administrador puede acceder a esta sección.", "OK");
            }
        }
    }
}

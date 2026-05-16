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
            Routing.RegisterRoute(nameof(CalendarioPedidosPage), typeof(CalendarioPedidosPage));
		}
    }
}

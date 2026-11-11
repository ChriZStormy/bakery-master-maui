using Microsoft.Maui.Controls;

namespace PasteleriaMaui.Views
{
	public partial class InicioPage : ContentPage
	{
		public InicioPage()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ActualizarVistaPorRol();
        }

        private void ActualizarVistaPorRol()
        {
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                BotonCatalogos.IsVisible = false;
                BotonUsuarios.IsVisible = false;
                BotonComandas.IsVisible = false;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Reclamos";
                BotonGestiones.Text = "Menú de Pasteles";
                BotonPedidos.Text = "Mis Pedidos";
            }
            else if (AppSession.CurrentUser != null && (AppSession.CurrentUser.Rol == "Repostero" || AppSession.CurrentUser.Rol == "Empleado"))
            {
                BotonCatalogos.IsVisible = false;
                BotonUsuarios.IsVisible = false;
                BotonComandas.IsVisible = true;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Reportes de Mermas";
                BotonGestiones.Text = "Gestión de Pasteles";
                BotonPedidos.Text = "Historial de Pedidos";
            }
            else
            {
                BotonCatalogos.IsVisible = true;
                BotonUsuarios.IsVisible = true;
                BotonComandas.IsVisible = true;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Reportes de Mermas";
                BotonGestiones.Text = "Gestión de Pasteles";
                BotonPedidos.Text = "Pedidos";
            }
        }
        private async void OnPersonalizarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PedidoPersonalizadoPage));
        }
		private async void OnPastelesClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync(nameof(ListaPastelesPage));
		}
		private async void OnPedidosClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync(nameof(HistorialPedidosPage));
		}
		private async void OnReportesClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync(nameof(ReporteMermaPage));
		}
		private async void OnCatalogosClicked(object sender, EventArgs e)
		{
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                await DisplayAlert("Acceso Denegado", "Solo los administradores pueden gestionar catálogos.", "OK");
                return;
            }
			await Shell.Current.GoToAsync(nameof(CatalogosPage));
		}

        private async void OnUsuariosClicked(object sender, EventArgs e)
        {
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                await DisplayAlert("Acceso Denegado", "Solo los administradores pueden gestionar usuarios.", "OK");
                return;
            }
            await Shell.Current.GoToAsync(nameof(GestionUsuariosPage));
        }

        private async void OnComandasClicked(object sender, EventArgs e)
        {
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                await DisplayAlert("Acceso Denegado", "Solo el personal de repostería y administración puede acceder a la comanda.", "OK");
                return;
            }
            await Shell.Current.GoToAsync(nameof(ComandaPage));
        }

        private void OnSalirClicked(object sender, EventArgs e)
        {
            AppSession.Clear();
            SecureStorage.Remove("jwt_token");
            Application.Current.MainPage = new LoginPage();
        }
	}
}

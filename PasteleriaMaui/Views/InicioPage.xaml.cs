
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
            var rol = AppSession.CurrentUser?.Rol ?? "Cliente";
            if (rol == "Cliente")
            {
                BotonGestiones.IsVisible = true;
                BotonGestiones.Text = "Menú de Pasteles";
                BotonPedidos.IsVisible = true;
                BotonPedidos.Text = "Mis Pedidos";
                BotonMermas.IsVisible = false;
                BotonCatalogos.IsVisible = false;
            }
            else if (rol == "Repostero" || rol == "Empleado/Repostero")
            {
                BotonGestiones.IsVisible = false;
                BotonCatalogos.IsVisible = false;
                BotonPedidos.IsVisible = true;
                BotonPedidos.Text = "👨‍🍳 Comanda de Cocina (Taller)";
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "📋 Registro de Mermas";
            }
            else // Admin
            {
                BotonGestiones.IsVisible = true;
                BotonGestiones.Text = "Gestión de Pasteles";
                BotonPedidos.IsVisible = true;
                BotonPedidos.Text = "Gestión de Pedidos";
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Reportes de Mermas";
                BotonCatalogos.IsVisible = true;
                BotonCatalogos.Text = "Catálogos";
            }
            // Update debug button label
            BtnDebugRol.Text = $"🔄 {rol}";
        }

        private void OnDebugRolClicked(object sender, EventArgs e)
        {
            if (AppSession.CurrentUser == null) return;

            if (AppSession.CurrentUser.Rol == "Cliente")
            {
                AppSession.CurrentUser.Rol = "Repostero";
                AppSession.CurrentUser.Nombre = "Chef Repostero (Debug)";
            }
            else if (AppSession.CurrentUser.Rol == "Repostero" || AppSession.CurrentUser.Rol == "Empleado/Repostero")
            {
                AppSession.CurrentUser.Rol = "Admin";
                AppSession.CurrentUser.Nombre = "Admin Master (Debug)";
            }
            else
            {
                AppSession.CurrentUser.Rol = "Cliente";
                AppSession.CurrentUser.Nombre = "Cliente Juan (Debug)";
            }
            ActualizarVistaPorRol();
        }
		private async void OnPastelesClicked(object sender, EventArgs e)
		{
            if (AppSession.CurrentUser != null && (AppSession.CurrentUser.Rol == "Repostero" || AppSession.CurrentUser.Rol == "Empleado/Repostero"))
            {
                await DisplayAlert("Acceso Restringido", "Los reposteros operan directamente en la Comanda de Cocina.", "OK");
                return;
            }
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
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol != "Admin")
            {
                await DisplayAlert("Acceso Denegado", "Solo los administradores pueden gestionar catálogos.", "OK");
                return;
            }
			await Shell.Current.GoToAsync(nameof(CatalogosPage));
		}

        private void OnSalirClicked(object sender, EventArgs e)
        {
            AppSession.CurrentUser = null;
            Application.Current.MainPage = new LoginPage();
        }
	}
}

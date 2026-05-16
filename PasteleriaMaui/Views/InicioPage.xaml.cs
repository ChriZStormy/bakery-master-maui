
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
                BotonMermas.IsVisible = true;
                BotonGestiones.Text = "Menú";
                BotonPedidos.Text = "Realizar pedido";
            }
            else
            {
                BotonCatalogos.IsVisible = true;
                BotonMermas.IsVisible = true;
                BotonGestiones.Text = "Gestión de Pasteles";
                BotonPedidos.Text = "Pedidos";
            }
            // Update debug button label
            var rol = AppSession.CurrentUser?.Rol ?? "?";
            BtnDebugRol.Text = $"🔄 {rol}";
        }

        private void OnDebugRolClicked(object sender, EventArgs e)
        {
            if (AppSession.CurrentUser == null) return;

            if (AppSession.CurrentUser.Rol == "Admin")
            {
                AppSession.CurrentUser.Rol = "Cliente";
                AppSession.CurrentUser.Nombre = "Cliente Juan (Debug)";
            }
            else
            {
                AppSession.CurrentUser.Rol = "Admin";
                AppSession.CurrentUser.Nombre = "Admin Master (Debug)";
            }
            ActualizarVistaPorRol();
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

        private void OnSalirClicked(object sender, EventArgs e)
        {
            AppSession.CurrentUser = null;
            Application.Current.MainPage = new LoginPage();
        }
	}
}

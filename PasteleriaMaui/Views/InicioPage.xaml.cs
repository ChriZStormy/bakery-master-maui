using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

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
            if (AppSession.CurrentUser != null)
            {
                SaludoUsuarioLabel.Text = $"Bienvenido(a), {AppSession.CurrentUser.Nombre}";
            }

            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                BotonPersonalizar.IsVisible = true;
                BotonCatalogos.IsVisible = false;
                BotonUsuarios.IsVisible = false;
                BotonComandas.IsVisible = false;
                BotonReporteEntregas.IsVisible = false;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Reclamos";
                BotonGestiones.IsVisible = true;
                BotonGestiones.Text = "Catálogo de Pasteles";
                BotonPedidos.IsVisible = true;
                BotonPedidos.Text = "Mis Pedidos";
            }
            else if (AppSession.CurrentUser != null && (AppSession.CurrentUser.Rol == "Repostero" || AppSession.CurrentUser.Rol == "Empleado"))
            {
                BotonPersonalizar.IsVisible = false;
                BotonCatalogos.IsVisible = false;
                BotonUsuarios.IsVisible = false;
                BotonComandas.IsVisible = true;
                BotonReporteEntregas.IsVisible = false;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Registro de Mermas";
                BotonGestiones.IsVisible = false;
                BotonPedidos.IsVisible = false;
            }
            else
            {
                BotonPersonalizar.IsVisible = true;
                BotonCatalogos.IsVisible = true;
                BotonUsuarios.IsVisible = true;
                BotonComandas.IsVisible = true;
                BotonReporteEntregas.IsVisible = true;
                BotonMermas.IsVisible = true;
                BotonMermas.Text = "Control y Reportes de Mermas";
                BotonGestiones.IsVisible = true;
                BotonGestiones.Text = "Gestión de Pasteles";
                BotonPedidos.IsVisible = true;
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

		private async void OnReporteEntregasClicked(object sender, EventArgs e)
		{
            if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
            {
                await DisplayAlert("Acceso Denegado", "Solo los administradores pueden consultar el reporte de entregas.", "OK");
                return;
            }
			await Shell.Current.GoToAsync(nameof(ReporteEntregasPage));
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

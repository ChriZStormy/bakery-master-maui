using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views
{
    public partial class CarritoPage : ContentPage
    {
        public CarritoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefrescarCarrito();
        }

        private void RefrescarCarrito()
        {
            CarritoCollection.ItemsSource = null;
            CarritoCollection.ItemsSource = CarritoService.Instance.Items;
            TotalLabel.Text = $"Total: ${CarritoService.Instance.Total:N2} MXN";
        }

        private void OnSumarCantidadClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is CarritoItem item)
            {
                item.Cantidad++;
                RefrescarCarrito();
            }
        }

        private void OnRestarCantidadClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is CarritoItem item && item.Cantidad > 1)
            {
                item.Cantidad--;
                RefrescarCarrito();
            }
        }

        private void OnEliminarItemClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is CarritoItem item)
            {
                CarritoService.Instance.Eliminar(item);
                RefrescarCarrito();
            }
        }

        private async void OnConfirmarPedidoClicked(object sender, EventArgs e)
        {
            if (CarritoService.Instance.Items.Count == 0)
            {
                await DisplayAlert("Carrito", "Agrega al menos un pastel al carrito.", "OK");
                return;
            }

            var primerItem = CarritoService.Instance.Items.First();
            var navParam = new Dictionary<string, object>
            {
                { "PastelAComprar", primerItem.Pastel }
            };
            CarritoService.Instance.Limpiar();
            await Shell.Current.GoToAsync(nameof(RegistrarPedidoPage), navParam);
        }
    }
}

using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

[QueryProperty(nameof(PastelAComprar), "PastelAComprar")]
public partial class RegistrarPedidoPage : ContentPage
{
    private Pastel _pastel;
    public Pastel PastelAComprar
    {
        get => _pastel;
        set
        {
            _pastel = value;
            OnPropertyChanged();
        }
    }


	public RegistrarPedidoPage()
	{
		InitializeComponent();
        PickerFechaEntrega.MinimumDate = DateTime.Today;
        PickerFechaEntrega.Date = DateTime.Today.AddDays(2);
	}


    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (PastelAComprar == null) return;

        if (!int.TryParse(EntryCantidad.Text, out int cantidad) || cantidad <= 0)
        {
            await DisplayAlert("Error", "La cantidad debe ser mayor a cero.", "OK");
            return;

        }



        var transacciones = new TransaccionesService();

        var nuevoPedido = new Pedido
        {
            PastelId = PastelAComprar.Id,
            UsuarioId = AppSession.CurrentUser.Id,
            Cantidad = cantidad,
            FechaEntrega = (DateTime)PickerFechaEntrega.Date
        };



        var success = await transacciones.CrearPedidoAsync(nuevoPedido);

        if (success)
        {
            await DisplayAlert("Éxito", "Tu pedido ha sido creado. Puedes verlo en el historial.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", "Hubo un problema al crear tu pedido.", "OK");
        }


        
    }
}
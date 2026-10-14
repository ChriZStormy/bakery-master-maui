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
        var fechaMinima = DateTime.Today.AddDays(2);
        PickerFechaEntrega.MinimumDate = fechaMinima;
        PickerFechaEntrega.Date = fechaMinima;
        PickerModalidad.SelectedIndex = 0;
        PickerFranjaHoraria.SelectedIndex = 1;
	}

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (PastelAComprar == null) return;

        if (!int.TryParse(EntryCantidad.Text, out int cantidad) || cantidad <= 0)
        {
            await DisplayAlert("Error", "La cantidad debe ser mayor a cero.", "OK");
            return;
        }

        var fechaSeleccionada = PickerFechaEntrega.Date;
        var anticipacionMinima = DateTime.Today.AddDays(2);
        if (fechaSeleccionada < anticipacionMinima)
        {
            await DisplayAlert("Validación de Anticipación", "La elaboración artesanal requiere al menos 48 horas de anticipación.", "OK");
            return;
        }

        var modalidad = PickerModalidad.SelectedItem as string ?? "Entrega a domicilio";
        var horario = PickerFranjaHoraria.SelectedItem as string ?? "Mediodía (12:00 - 15:00)";

        var transacciones = new TransaccionesService();
        var nuevoPedido = new Pedido
        {
            PastelId = PastelAComprar.Id,
            UsuarioId = AppSession.CurrentUser?.Id ?? 2,
            Cantidad = cantidad,
            FechaEntrega = fechaSeleccionada
        };

        var success = await transacciones.CrearPedidoAsync(nuevoPedido);
        if (success)
        {
            var folio = nuevoPedido.Id > 0 ? $"#DV-{nuevoPedido.Id:D4}" : $"#DV-{DateTime.Now:fff}";
            await DisplayAlert("¡Orden Confirmada!", 
                $"Tu orden ha sido generada exitosamente.\n\n" +
                $"Folio único: {folio}\n" +
                $"Pastel: {PastelAComprar.Nombre}\n" +
                $"Cantidad: {cantidad} unidad(es)\n" +
                $"Modalidad: {modalidad}\n" +
                $"Fecha de entrega: {fechaSeleccionada:dd/MM/yyyy}\n" +
                $"Franja horaria: {horario}\n\n" +
                $"Puedes consultar el avance en tiempo real en 'Mis Pedidos'.", 
                "Aceptar");

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", "Hubo un problema al crear tu pedido. Verifica la conexión con el servidor.", "OK");
        }
    }
}
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
            ActualizarTotal();
        }
    }

	public RegistrarPedidoPage()
	{
		InitializeComponent();
        PickerFechaEntrega.MinimumDate = DateTime.Today;
        PickerFechaEntrega.Date = DateTime.Today.AddDays(2);
        EntryCantidad.TextChanged += (s, e) => ActualizarTotal();
	}

    private void ActualizarTotal()
    {
        if (PastelAComprar == null) return;
        decimal basePrice = PastelAComprar.Tamanio?.Trim().ToLower() switch
        {
            "chico" => 250m,
            "mediano" => 350m,
            "grande" => 480m,
            "extra grande" => 620m,
            _ => 350m
        };
        int cant = int.TryParse(EntryCantidad.Text, out int c) && c > 0 ? c : 1;
        decimal total = basePrice * cant;
        LblTotalEstimado.Text = $"Total a pagar: ${total:N2} MXN ({cant} ud • {PastelAComprar.Tamanio})";
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (PastelAComprar == null) return;

        if (!int.TryParse(EntryCantidad.Text, out int cantidad) || cantidad <= 0)
        {
            await DisplayAlert("Error", "La cantidad debe ser mayor a cero.", "OK");
            return;
        }

        DateTime fechaSeleccionada = PickerFechaEntrega.Date ?? DateTime.Today;
        TimeSpan horaSeleccionada = PickerHoraEntrega.Time ?? new TimeSpan(14, 0, 0);
        DateTime fechaHoraEntrega = fechaSeleccionada.Date.Add(horaSeleccionada);

        var transacciones = new TransaccionesService();

        var nuevoPedido = new Pedido
        {
            PastelId = PastelAComprar.Id,
            UsuarioId = AppSession.CurrentUser?.Id ?? 2,
            Cantidad = cantidad,
            FechaEntrega = fechaHoraEntrega,
            Estatus = "Pendiente"
        };

        var success = await transacciones.CrearPedidoAsync(nuevoPedido);

        if (success)
        {
            await DisplayAlert("¡Pedido Exitoso!", "Tu pedido ha sido creado y enviado al taller de repostería. Se ha generado tu comprobante y se envió a tu correo electrónico.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", "Hubo un problema al crear tu pedido.", "OK");
        }
    }
}
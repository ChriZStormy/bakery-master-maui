using System.Collections.ObjectModel;
using System.Linq;
using PasteleriaMaui.Services;
using PasteleriaMaui.Models;

namespace PasteleriaMaui.Views;

public partial class ReporteMermaPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;
    private readonly PastelApiService _pastelService;

    public ObservableCollection<Merma> Mermas { get; set; } = new ObservableCollection<Merma>();

    public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";

	public ReporteMermaPage()
	{
		InitializeComponent();
        _transaccionesService = new TransaccionesService();
        _pastelService = new PastelApiService();
        BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        int? filterId = null;
        if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Admin")
        {
            FormularioMerma.IsVisible = false; 
        }
        else 
        {
            FormularioMerma.IsVisible = true;
            filterId = AppSession.CurrentUser?.Id;
        }

        var mermasApi = await _transaccionesService.GetMermasAsync(filterId);
        Mermas.Clear();
        foreach (var m in mermasApi)
        {
            Mermas.Add(m);
        }

        if (AppSession.CurrentUser?.Rol == "Cliente")
        {
            var misPedidos = await _transaccionesService.GetPedidosAsync(AppSession.CurrentUser.Id);
            var pastelesComprados = misPedidos.Select(p => p.Pastel).GroupBy(p => p.Id).Select(g => g.First()).ToList();
            PickerPastel.ItemsSource = pastelesComprados;
        }
        else
        {
            var pastelesApi = await _pastelService.GetPastelesAsync();
            PickerPastel.ItemsSource = pastelesApi;
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (PickerPastel.SelectedItem is Pastel pastelSeleccionado && !string.IsNullOrWhiteSpace(DescripcionMerma.Text))
        {
            var nuevaMerma = new Merma
            {
                PastelId = pastelSeleccionado.Id,
                UsuarioId = AppSession.CurrentUser.Id,
                Fecha = (DateTime)FechaMerma.Date,
                Descripcion = DescripcionMerma.Text
            };
            
            var exito = await _transaccionesService.CrearMermaAsync(nuevaMerma);
            if (exito)
            {
                await DisplayAlert("Éxito", "Merma guardada en el sistema.", "OK");
                DescripcionMerma.Text = string.Empty;
                PickerPastel.SelectedItem = null;
                await CargarDatos();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar la merma.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Advertencia", "Por favor selecciona un pastel y escribe la descripción.", "OK");
        }
    }

    private async void OnContactarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is Merma merma)
        {
            if (!string.IsNullOrEmpty(merma.Usuario?.Email))
            {
                await Launcher.OpenAsync(new Uri($"mailto:{merma.Usuario.Email}"));
            }
            else
            {
                await DisplayAlert("Sin contacto", "El cliente no tiene un correo registrado.", "OK");
            }
        }
    }

    private async void OnAtenderClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is Merma merma)
        {
            if (merma.Estatus == "Atendida")
            {
                await DisplayAlert("Info", "Esta merma ya está atendida.", "OK");
                return;
            }

            merma.Estatus = "Atendida";
            var exito = await _transaccionesService.ActualizarMermaAsync(merma);
            if (exito)
            {
                await DisplayAlert("Éxito", "Merma marcada como atendida.", "OK");
                await CargarDatos(); // Refrescar lista
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar el estatus.", "OK");
            }
        }
    }
}
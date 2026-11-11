using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

public partial class ReporteMermaPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;
    private readonly PastelApiService _pastelService;

    public ObservableCollection<Merma> Mermas { get; set; } = new ObservableCollection<Merma>();
    private List<Merma> _todasLasMermas = new List<Merma>();

    public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";
    public bool IsRepostero => AppSession.CurrentUser?.Rol == "Repostero" || AppSession.CurrentUser?.Rol == "Empleado";
    public bool IsCliente => AppSession.CurrentUser?.Rol == "Cliente";

    public ReporteMermaPage()
    {
        InitializeComponent();
        _transaccionesService = new TransaccionesService();
        _pastelService = new PastelApiService();
        BindingContext = this;

        // Configuración inicial de controles
        FiltroFechaInicio.Date = DateTime.Today.AddDays(-30);
        FiltroFechaFin.Date = DateTime.Today;
        FechaMerma.Date = DateTime.Today;
        PickerTipoElemento.SelectedIndex = 0; // Pastel por defecto
        PickerUnidad.SelectedIndex = 0; // Piezas
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ConfigurarVistaPorRol();
        await CargarCatalogos();
        await CargarDatos();
    }

    private void ConfigurarVistaPorRol()
    {
        if (IsCliente)
        {
            LabelTituloModulo.Text = "Mis Reclamos y Reportes";
            BarraTabs.IsVisible = false;
            ActivarTab("registro");
            SeccionRegistro.IsVisible = true;
            SeccionAnalitico.IsVisible = false;
            SeccionHistorial.IsVisible = true;
        }
        else if (IsRepostero)
        {
            LabelTituloModulo.Text = "Registro y Control de Mermas de Taller";
            BarraTabs.IsVisible = true;
            TabBtnAnalitico.IsVisible = false; // Solo Administrador ve analítico
            ActivarTab("registro");
        }
        else // Admin
        {
            LabelTituloModulo.Text = "Control Analítico de Mermas";
            BarraTabs.IsVisible = true;
            TabBtnAnalitico.IsVisible = true;
            ActivarTab("registro");
        }
    }

    private async Task CargarCatalogos()
    {
        try
        {
            if (IsCliente)
            {
                var misPedidos = await _transaccionesService.GetPedidosAsync(AppSession.CurrentUser?.Id);
                var pastelesComprados = misPedidos.Select(p => p.Pastel).Where(p => p != null).GroupBy(p => p!.Id).Select(g => g.First()!).ToList();
                PickerPastel.ItemsSource = pastelesComprados;
            }
            else
            {
                var pasteles = await _pastelService.GetPastelesAsync();
                PickerPastel.ItemsSource = pasteles;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar pasteles: {ex.Message}");
        }
    }

    private async Task CargarDatos()
    {
        try
        {
            string? filterId = IsCliente ? AppSession.CurrentUser?.Id : null;
            var mermasApi = await _transaccionesService.GetMermasAsync(filterId);

            _todasLasMermas = mermasApi ?? new List<Merma>();

            AplicarFiltrosLocales();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar mermas: {ex.Message}");
        }
    }

    #region Manejo de Pestañas
    private void OnTabRegistroClicked(object sender, EventArgs e) => ActivarTab("registro");
    private void OnTabAnaliticoClicked(object sender, EventArgs e) => ActivarTab("analitico");
    private void OnTabHistorialClicked(object sender, EventArgs e) => ActivarTab("historial");

    private void ActivarTab(string tab)
    {
        SeccionRegistro.IsVisible = (tab == "registro");
        SeccionAnalitico.IsVisible = (tab == "analitico");
        SeccionHistorial.IsVisible = (tab == "historial");

        // Estilos de botones de pestaña
        ActualizarEstiloBotonTab(TabBtnRegistro, tab == "registro");
        ActualizarEstiloBotonTab(TabBtnAnalitico, tab == "analitico");
        ActualizarEstiloBotonTab(TabBtnHistorial, tab == "historial");

        if (tab == "analitico")
        {
            CalcularMetricasAnaliticas(Mermas.ToList());
        }
    }

    private void ActualizarEstiloBotonTab(Button boton, bool activo)
    {
        if (boton == null) return;
        boton.BackgroundColor = activo ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        boton.TextColor = activo ? Colors.White : Color.FromArgb("#5D4037");
    }
    #endregion

    #region Formulario de Registro Dinámico
    private void OnTipoElementoChanged(object sender, EventArgs e)
    {
        bool esPastel = PickerTipoElemento.SelectedIndex == 0;
        LayoutPastel.IsVisible = esPastel;
        LayoutInsumo.IsVisible = !esPastel;

        if (esPastel)
        {
            PickerUnidad.SelectedItem = "Piezas";
            LayoutOtroInsumo.IsVisible = false;
        }
        else
        {
            PickerUnidad.SelectedItem = "Kg";
        }
        RecalcularCostoEstimado();
    }

    private void OnPastelSeleccionadoChanged(object sender, EventArgs e)
    {
        RecalcularCostoEstimado();
    }

    private void OnInsumoSeleccionadoChanged(object sender, EventArgs e)
    {
        string? insumo = PickerInsumo.SelectedItem as string;
        LayoutOtroInsumo.IsVisible = (insumo == "Otro Insumo no listado");

        if (insumo != null)
        {
            if (insumo.Contains("(Kg)")) PickerUnidad.SelectedItem = "Kg";
            else if (insumo.Contains("(Litros)")) PickerUnidad.SelectedItem = "Litros";
            else if (insumo.Contains("(Piezas)") || insumo.Contains("(Pieza)")) PickerUnidad.SelectedItem = "Piezas";
        }
        RecalcularCostoEstimado();
    }

    private void OnCantidadOElementoChanged(object sender, TextChangedEventArgs e)
    {
        RecalcularCostoEstimado();
    }

    private decimal ObtenerCostoEstimadoInsumo(string insumo)
    {
        if (string.IsNullOrEmpty(insumo)) return 50.00m;
        if (insumo.Contains("Harina")) return 28.00m;
        if (insumo.Contains("Azúcar")) return 35.00m;
        if (insumo.Contains("Mantequilla")) return 140.00m;
        if (insumo.Contains("Huevos")) return 4.50m;
        if (insumo.Contains("Leche")) return 26.00m;
        if (insumo.Contains("Crema")) return 85.00m;
        if (insumo.Contains("Chocolate")) return 180.00m;
        if (insumo.Contains("Frutos")) return 120.00m;
        if (insumo.Contains("Bizcocho")) return 110.00m;
        if (insumo.Contains("Relleno")) return 90.00m;
        if (insumo.Contains("Glaseado")) return 75.00m;
        return 50.00m;
    }

    private void RecalcularCostoEstimado()
    {
        double.TryParse(EntryCantidad.Text, out double cantidad);
        if (cantidad <= 0) cantidad = 1;

        if (PickerTipoElemento.SelectedIndex == 0 && PickerPastel.SelectedItem is Pastel pastel)
        {
            decimal precioBase = pastel.Tamanio?.ToLowerInvariant() switch
            {
                "chico" => 250m,
                "mediano" => 380m,
                "grande" => 520m,
                "extra grande" => 680m,
                _ => 380m
            };
            EntryCostoEstimado.Text = (precioBase * (decimal)cantidad).ToString("0.00", CultureInfo.InvariantCulture);
        }
        else if (PickerTipoElemento.SelectedIndex == 1 && PickerInsumo.SelectedItem is string insumo)
        {
            decimal costoKg = ObtenerCostoEstimadoInsumo(insumo);
            EntryCostoEstimado.Text = (costoKg * (decimal)cantidad).ToString("0.00", CultureInfo.InvariantCulture);
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        bool esPastel = PickerTipoElemento.SelectedIndex == 0;
        string elementoAfectado = string.Empty;
        int? pastelId = null;

        if (esPastel)
        {
            if (PickerPastel.SelectedItem is Pastel pastel)
            {
                elementoAfectado = pastel.Nombre;
                pastelId = pastel.Id;
            }
            else
            {
                await DisplayAlert("Validación", "Por favor selecciona un pastel del catálogo.", "OK");
                return;
            }
        }
        else
        {
            if (PickerInsumo.SelectedItem is string insumo)
            {
                if (insumo == "Otro Insumo no listado")
                {
                    if (string.IsNullOrWhiteSpace(EntryOtroInsumo.Text))
                    {
                        await DisplayAlert("Validación", "Por favor escribe el nombre del insumo.", "OK");
                        return;
                    }
                    elementoAfectado = EntryOtroInsumo.Text.Trim();
                }
                else
                {
                    elementoAfectado = insumo;
                }
            }
            else
            {
                await DisplayAlert("Validación", "Por favor selecciona un insumo de materia prima.", "OK");
                return;
            }
        }

        if (!double.TryParse(EntryCantidad.Text, out double cantidad) || cantidad <= 0)
        {
            await DisplayAlert("Validación", "Por favor ingresa una cantidad o peso válido mayor a 0.", "OK");
            return;
        }

        string causaSeleccionada = PickerCausa.SelectedItem as string ?? string.Empty;
        if (string.IsNullOrWhiteSpace(causaSeleccionada))
        {
            await DisplayAlert("Validación", "Por favor selecciona la causa predefinida de la merma.", "OK");
            return;
        }

        decimal.TryParse(EntryCostoEstimado.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal costoEstimado);
        string unidad = PickerUnidad.SelectedItem as string ?? (esPastel ? "Piezas" : "Kg");

        var nuevaMerma = new Merma
        {
            TipoElemento = esPastel ? "Pastel" : "Insumo",
            PastelId = pastelId,
            ElementoAfectado = elementoAfectado,
            Cantidad = cantidad,
            UnidadMedida = unidad,
            Causa = causaSeleccionada,
            CostoEstimado = costoEstimado,
            Fecha = FechaMerma.Date ?? DateTime.Today,
            Descripcion = DescripcionMerma.Text?.Trim(),
            UsuarioId = AppSession.CurrentUser?.Id,
            Estatus = "Registrada"
        };

        var exito = await _transaccionesService.CrearMermaAsync(nuevaMerma);
        if (exito)
        {
            await DisplayAlert("Éxito", $"Merma de {elementoAfectado} registrada correctamente.", "OK");
            
            // Limpiar formulario
            DescripcionMerma.Text = string.Empty;
            EntryCantidad.Text = "1";
            PickerPastel.SelectedItem = null;
            PickerInsumo.SelectedItem = null;
            PickerCausa.SelectedItem = null;
            EntryOtroInsumo.Text = string.Empty;
            FechaMerma.Date = DateTime.Today;

            await CargarDatos();

            // Cambiar a la pestaña de historial para ver el nuevo registro
            ActivarTab("historial");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar la merma en el servidor.", "OK");
        }
    }
    #endregion

    #region Reporte Analítico y Métricas
    private void OnFiltroSemanalClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddDays(-7);
        FiltroFechaFin.Date = DateTime.Today;
        AplicarFiltrosLocales();
    }

    private void OnFiltroMensualClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddDays(-30);
        FiltroFechaFin.Date = DateTime.Today;
        AplicarFiltrosLocales();
    }

    private void OnFiltroTodoClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddYears(-2);
        FiltroFechaFin.Date = DateTime.Today.AddDays(1);
        if (FiltroCausaPicker != null) FiltroCausaPicker.SelectedIndex = 0;
        AplicarFiltrosLocales();
    }

    private void OnAplicarFiltrosClicked(object sender, EventArgs e)
    {
        AplicarFiltrosLocales();
    }

    private void AplicarFiltrosLocales()
    {
        var filtradas = _todasLasMermas.AsEnumerable();

        // Filtro por fecha
        DateTime inicio = (FiltroFechaInicio.Date ?? DateTime.Today.AddDays(-30)).Date;
        DateTime fin = (FiltroFechaFin.Date ?? DateTime.Today).Date.AddDays(1).AddTicks(-1);
        filtradas = filtradas.Where(m => m.Fecha >= inicio && m.Fecha <= fin);

        // Filtro por causa
        string? causa = FiltroCausaPicker?.SelectedItem as string;
        if (!string.IsNullOrEmpty(causa) && causa != "Todas las causas")
        {
            filtradas = filtradas.Where(m => m.Causa == causa);
        }

        var lista = filtradas.OrderByDescending(m => m.Fecha).ToList();

        Mermas.Clear();
        foreach (var m in lista)
        {
            Mermas.Add(m);
        }

        LblSinRegistros.IsVisible = (Mermas.Count == 0);

        CalcularMetricasAnaliticas(lista);
    }

    private void CalcularMetricasAnaliticas(List<Merma> lista)
    {
        int totalIncidentes = lista.Count;
        double cantidadTotal = lista.Sum(m => m.Cantidad);
        decimal costoTotal = lista.Sum(m => m.CostoEstimado);

        LabelResumenIncidentes.Text = totalIncidentes.ToString();
        LabelResumenCantidad.Text = $"{cantidadTotal:0.##}";
        LabelResumenCosto.Text = costoTotal.ToString("C2", CultureInfo.GetCultureInfo("es-MX"));

        // Métricas por Causa
        int caducidadCount = lista.Count(m => m.Causa == "Caducidad de insumo");
        int horneadoCount = lista.Count(m => m.Causa == "Error de horneado/quemado");
        int danioCount = lista.Count(m => m.Causa == "Daño durante decoración/transporte");
        int otroCount = lista.Count(m => m.Causa == "Otro" || (m.Causa != "Caducidad de insumo" && m.Causa != "Error de horneado/quemado" && m.Causa != "Daño durante decoración/transporte"));

        double pCaducidad = totalIncidentes > 0 ? (double)caducidadCount / totalIncidentes : 0;
        double pHorneado = totalIncidentes > 0 ? (double)horneadoCount / totalIncidentes : 0;
        double pDanio = totalIncidentes > 0 ? (double)danioCount / totalIncidentes : 0;
        double pOtro = totalIncidentes > 0 ? (double)otroCount / totalIncidentes : 0;

        ProgressCaducidad.Progress = pCaducidad;
        LblStatCaducidad.Text = $"{caducidadCount} ({pCaducidad:P0})";

        ProgressHorneado.Progress = pHorneado;
        LblStatHorneado.Text = $"{horneadoCount} ({pHorneado:P0})";

        ProgressDanio.Progress = pDanio;
        LblStatDanio.Text = $"{danioCount} ({pDanio:P0})";

        ProgressOtro.Progress = pOtro;
        LblStatOtro.Text = $"{otroCount} ({pOtro:P0})";
    }
    #endregion

    #region Acciones Administrativas
    private async void OnContactarClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Merma merma)
        {
            if (!string.IsNullOrEmpty(merma.Usuario?.Email))
            {
                await Launcher.OpenAsync(new Uri($"mailto:{merma.Usuario.Email}"));
            }
            else
            {
                await DisplayAlert("Sin contacto", "El usuario no tiene un correo registrado.", "OK");
            }
        }
    }

    private async void OnAtenderClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Merma merma)
        {
            if (merma.Estatus == "Atendida")
            {
                await DisplayAlert("Info", "Esta merma ya está marcada como atendida.", "OK");
                return;
            }

            merma.Estatus = "Atendida";
            var exito = await _transaccionesService.ActualizarMermaAsync(merma);
            if (exito)
            {
                await DisplayAlert("Éxito", "Merma actualizada correctamente.", "OK");
                await CargarDatos();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar el estatus.", "OK");
            }
        }
    }
    #endregion
}

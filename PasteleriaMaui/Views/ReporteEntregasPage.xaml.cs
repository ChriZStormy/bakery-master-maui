using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views;

public partial class ReporteEntregasPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;
    public ObservableCollection<Pedido> Pedidos { get; set; } = new ObservableCollection<Pedido>();
    private ReporteEntregasDto? _reporteActual;

    public ReporteEntregasPage()
    {
        InitializeComponent();
        _transaccionesService = new TransaccionesService();
        BindingContext = this;

        FiltroFechaInicio.Date = DateTime.Today.AddDays(-30);
        FiltroFechaFin.Date = DateTime.Today;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarReporteAsync();
    }

    private async Task CargarReporteAsync()
    {
        try
        {
            DateTime inicio = (FiltroFechaInicio.Date ?? DateTime.Today.AddDays(-30)).Date;
            DateTime fin = (FiltroFechaFin.Date ?? DateTime.Today).Date;

            var reporte = await _transaccionesService.GetReporteEntregasAsync(inicio, fin);

            if (reporte != null)
            {
                _reporteActual = reporte;
                ActualizarInterfazConReporte(reporte);
            }
            else
            {
                // Fallback local en caso de conexión directa
                var todosPedidos = await _transaccionesService.GetPedidosAsync();
                var filtrados = todosPedidos
                    .Where(p => p.Fecha.Date >= inicio && p.Fecha.Date <= fin)
                    .OrderByDescending(p => p.FechaEntrega)
                    .ToList();

                int total = filtrados.Count;
                int aTiempo = filtrados.Count(p => p.EsATiempo && (p.Estatus == "Entregado" || p.Estatus == "Completado"));
                int retrasados = filtrados.Count(p => !p.EsATiempo || (DateTime.Now > p.FechaEntrega && p.Estatus != "Entregado" && p.Estatus != "Completado"));
                int despachados = aTiempo + retrasados;
                double pct = despachados > 0 ? ((double)aTiempo / despachados) * 100 : 100;
                double promHoras = filtrados.Count > 0 ? filtrados.Average(p => p.HorasPreparacion) : 0;

                _reporteActual = new ReporteEntregasDto
                {
                    TotalPedidos = total,
                    EntregadosATiempo = aTiempo,
                    EntregasRetrasadas = retrasados,
                    EnProceso = total - despachados,
                    TiempoPromedioPreparacionHoras = Math.Round(promHoras, 1),
                    PorcentajePuntualidad = Math.Round(pct, 1),
                    Pedidos = filtrados
                };

                ActualizarInterfazConReporte(_reporteActual);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar reporte de entregas: {ex.Message}");
        }
    }

    private void ActualizarInterfazConReporte(ReporteEntregasDto reporte)
    {
        LblTotalATiempo.Text = reporte.EntregadosATiempo.ToString();
        LblTotalRetrasados.Text = reporte.EntregasRetrasadas.ToString();
        LblPromedioPreparacion.Text = $"{reporte.TiempoPromedioPreparacionHoras:0.#} hrs";

        int despachados = reporte.EntregadosATiempo + reporte.EntregasRetrasadas;
        double pctATiempo = despachados > 0 ? ((double)reporte.EntregadosATiempo / despachados) * 100 : 100;
        double pctRetrasados = despachados > 0 ? ((double)reporte.EntregasRetrasadas / despachados) * 100 : 0;

        LblPctATiempo.Text = $"{pctATiempo:0.#}% puntualidad";
        LblPctRetrasados.Text = $"{pctRetrasados:0.#}% fuera de tiempo";

        LblIndicePuntualidad.Text = $"{reporte.PorcentajePuntualidad:0.#}%";
        ProgressPuntualidad.Progress = Math.Clamp(reporte.PorcentajePuntualidad / 100.0, 0.0, 1.0);

        Pedidos.Clear();
        foreach (var p in reporte.Pedidos)
        {
            Pedidos.Add(p);
        }

        LblSinPedidos.IsVisible = (Pedidos.Count == 0);
    }

    #region Filtros Rápidos
    private async void OnFiltroSemanalClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddDays(-7);
        FiltroFechaFin.Date = DateTime.Today;
        await CargarReporteAsync();
    }

    private async void OnFiltroMensualClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddDays(-30);
        FiltroFechaFin.Date = DateTime.Today;
        await CargarReporteAsync();
    }

    private async void OnFiltroTodoClicked(object sender, EventArgs e)
    {
        FiltroFechaInicio.Date = DateTime.Today.AddYears(-2);
        FiltroFechaFin.Date = DateTime.Today.AddDays(1);
        await CargarReporteAsync();
    }

    private async void OnActualizarReporteClicked(object sender, EventArgs e)
    {
        await CargarReporteAsync();
    }
    #endregion

    #region Exportación y Vista Imprimible
    private async void OnExportarClicked(object sender, EventArgs e)
    {
        if (_reporteActual == null || _reporteActual.Pedidos.Count == 0)
        {
            await DisplayAlert("Sin datos", "No hay datos en el reporte para exportar.", "OK");
            return;
        }

        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("REPORTE DE CUMPLIMIENTO DE ENTREGAS Y DESPACHOS - PASTELERIA DOLCE VITA");
            sb.AppendLine($"Fecha de Generación: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Período: {FiltroFechaInicio.Date:dd/MM/yyyy} a {FiltroFechaFin.Date:dd/MM/yyyy}");
            sb.AppendLine($"Total de Pedidos Analizados: {_reporteActual.TotalPedidos}");
            sb.AppendLine($"Entregados a Tiempo: {_reporteActual.EntregadosATiempo} ({_reporteActual.PorcentajePuntualidad:0.#}%)");
            sb.AppendLine($"Entregas Retrasadas: {_reporteActual.EntregasRetrasadas}");
            sb.AppendLine($"Tiempo Promedio de Preparación: {_reporteActual.TiempoPromedioPreparacionHoras} horas");
            sb.AppendLine();
            sb.AppendLine("ID_Orden,Fecha_Creacion,Fecha_Programada,Cliente,Pastel,Cantidad,Total,Modalidad,Estado_Puntualidad,Horas_Preparacion");

            foreach (var p in _reporteActual.Pedidos)
            {
                sb.AppendLine($"{p.Id},{p.Fecha:yyyy-MM-dd},{p.FechaEntrega:yyyy-MM-dd},\"{p.Usuario?.Nombre}\",\"{p.Pastel?.Nombre}\",{p.Cantidad},{p.Total.ToString("F2", CultureInfo.InvariantCulture)},\"{p.ModalidadEntrega}\",\"{p.EstadoPuntualidadTexto}\",{p.HorasPreparacion.ToString("F1", CultureInfo.InvariantCulture)}");
            }

            string nombreArchivo = $"Reporte_Entregas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            string rutaLocal = Path.Combine(FileSystem.CacheDirectory, nombreArchivo);
            await File.WriteAllTextAsync(rutaLocal, sb.ToString(), Encoding.UTF8);

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Descargar Reporte de Entregas",
                File = new ShareFile(rutaLocal)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Exportación", $"No se pudo exportar el archivo: {ex.Message}", "OK");
        }
    }

    private async void OnVistaImprimibleClicked(object sender, EventArgs e)
    {
        if (_reporteActual == null)
        {
            await DisplayAlert("Info", "Genera el reporte antes de abrir la vista imprimible.", "OK");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("═════════════════════════════════════════");
        sb.AppendLine("  PASTELERÍA DOLCE VITA - LOGÍSTICA");
        sb.AppendLine("  REPORTE DE CUMPLIMIENTO DE ENTREGAS");
        sb.AppendLine("═════════════════════════════════════════");
        sb.AppendLine($"Rango: {FiltroFechaInicio.Date:dd/MM/yyyy} - {FiltroFechaFin.Date:dd/MM/yyyy}");
        sb.AppendLine($"• Total de Pedidos: {_reporteActual.TotalPedidos}");
        sb.AppendLine($"• Entregas a Tiempo: {_reporteActual.EntregadosATiempo}");
        sb.AppendLine($"• Entregas Retrasadas: {_reporteActual.EntregasRetrasadas}");
        sb.AppendLine($"• Puntualidad Global: {_reporteActual.PorcentajePuntualidad:0.#}%");
        sb.AppendLine($"• Tiempo Promedio Prep.: {_reporteActual.TiempoPromedioPreparacionHoras} hrs");
        sb.AppendLine("─────────────────────────────────────────");
        sb.AppendLine("DETALLE DE DESPACHOS:");

        int i = 1;
        foreach (var p in _reporteActual.Pedidos.Take(15))
        {
            sb.AppendLine($"{i++}. Orden #{p.Id} | {p.Pastel?.Nombre} | {p.EstadoPuntualidadTexto} ({p.HorasPreparacion} hrs prep.)");
        }
        if (_reporteActual.Pedidos.Count > 15)
        {
            sb.AppendLine($"... y {_reporteActual.Pedidos.Count - 15} pedidos adicionales (disponibles en el CSV).");
        }
        sb.AppendLine("═════════════════════════════════════════");

        await DisplayAlert("Vista Imprimible de Entregas", sb.ToString(), "Cerrar");
    }
    #endregion
}

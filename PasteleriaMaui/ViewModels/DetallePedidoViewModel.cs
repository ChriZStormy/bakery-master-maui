using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    public class DetallePedidoViewModel : INotifyPropertyChanged
    {
        private readonly TransaccionesService _transaccionesService;
        private Pedido _pedido;
        private string _nuevoEstatus;
        private string _ultimaActualizacionTexto;

        public Pedido Pedido
        {
            get => _pedido;
            set 
            { 
                _pedido = value; 
                OnPropertyChanged();
                ActualizarEtapas();
            }
        }

        public string NuevoEstatus
        {
            get => _nuevoEstatus;
            set { _nuevoEstatus = value; OnPropertyChanged(); }
        }

        public bool IsAdmin => AppSession.CurrentUser?.Rol == "Admin";

        public string FolioFormateado => Pedido != null ? $"#DV-{Pedido.Id:D4}" : "#DV-0000";

        // Propiedades de la barra de avance gráfica y etapas en tiempo real
        public double ProgresoPorcentaje { get; private set; } = 0.25;
        public string IconoEstatusActual { get; private set; } = "📋";
        public string TituloEstatusActual { get; private set; } = "Recibido";
        public string DescripcionEstatusActual { get; private set; } = "Tu orden ha sido registrada en el sistema de la pastelería.";

        public string Fase1BgColor { get; private set; } = "#8B5E34";
        public string Fase1TextColor { get; private set; } = "#FFFFFF";
        public string Fase1Icono { get; private set; } = "✓";
        public string Fase1Fecha { get; private set; } = "";

        public string Fase2BgColor { get; private set; } = "#EAE4DB";
        public string Fase2TextColor { get; private set; } = "#A1887F";
        public string Fase2Icono { get; private set; } = "2";
        public string Fase2Fecha { get; private set; } = "Pendiente";

        public string Fase3BgColor { get; private set; } = "#EAE4DB";
        public string Fase3TextColor { get; private set; } = "#A1887F";
        public string Fase3Icono { get; private set; } = "3";
        public string Fase3Fecha { get; private set; } = "Pendiente";

        public string Fase4BgColor { get; private set; } = "#EAE4DB";
        public string Fase4TextColor { get; private set; } = "#A1887F";
        public string Fase4Icono { get; private set; } = "4";
        public string Fase4Fecha { get; private set; } = "Pendiente";

        public bool IsCancelado { get; private set; } = false;

        public string UltimaActualizacionTexto
        {
            get => _ultimaActualizacionTexto;
            private set { _ultimaActualizacionTexto = value; OnPropertyChanged(); }
        }

        public ICommand VolverCommand { get; set; }
        public ICommand ActualizarEstatusCommand { get; set; }
        public ICommand RefrescarEstadoCommand { get; set; }

        public DetallePedidoViewModel(Pedido pedidoSeleccionado, INavigation navigation)
        {
            _transaccionesService = new TransaccionesService();
            Pedido = pedidoSeleccionado;
            NuevoEstatus = Pedido.Estatus;

            VolverCommand = new Command(async () => await navigation.PopAsync());
            ActualizarEstatusCommand = new Command(async () => await ActualizarEstatus());
            RefrescarEstadoCommand = new Command(async () => await RefrescarEstadoEnTiempoReal());

            ActualizarEtapas();
            UltimaActualizacionTexto = $"Consultado en vivo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        private int ObtenerNumeroFase(string? estatus)
        {
            if (string.IsNullOrWhiteSpace(estatus)) return 1;
            var est = estatus.Trim();

            if (est.Equals("Recibido", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                return 1;

            if (est.Equals("En Preparación", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("En Preparación/Horneado", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("En Proceso", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Horneado", StringComparison.OrdinalIgnoreCase))
                return 2;

            if (est.Equals("Decorado", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Listo", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Listo para entrega", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Decorado/Listo para entrega", StringComparison.OrdinalIgnoreCase))
                return 3;

            if (est.Equals("Entregado", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Entregados", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Completado", StringComparison.OrdinalIgnoreCase))
                return 4;

            if (est.Equals("Cancelado", StringComparison.OrdinalIgnoreCase) ||
                est.Equals("Cancelados", StringComparison.OrdinalIgnoreCase))
                return -1;

            return 1;
        }

        public void ActualizarEtapas()
        {
            if (Pedido == null) return;

            int fase = ObtenerNumeroFase(Pedido.Estatus);
            IsCancelado = (fase == -1);

            Fase1Fecha = Pedido.Fecha != DateTime.MinValue ? Pedido.Fecha.ToString("dd/MM HH:mm") : "";
            Fase4Fecha = Pedido.FechaEntrega != DateTime.MinValue ? Pedido.FechaEntrega.ToString("dd/MM") : "Entrega";

            if (IsCancelado)
            {
                ProgresoPorcentaje = 0;
                IconoEstatusActual = "❌";
                TituloEstatusActual = "Pedido Cancelado";
                DescripcionEstatusActual = "Este pedido ha sido cancelado por la pastelería o solicitud del cliente.";

                Fase1BgColor = "#EAE4DB"; Fase1TextColor = "#A1887F"; Fase1Icono = "1";
                Fase2BgColor = "#EAE4DB"; Fase2TextColor = "#A1887F"; Fase2Icono = "2"; Fase2Fecha = "Cancelado";
                Fase3BgColor = "#EAE4DB"; Fase3TextColor = "#A1887F"; Fase3Icono = "3"; Fase3Fecha = "Cancelado";
                Fase4BgColor = "#EAE4DB"; Fase4TextColor = "#A1887F"; Fase4Icono = "4"; Fase4Fecha = "Cancelado";
            }
            else
            {
                switch (fase)
                {
                    case 1:
                        ProgresoPorcentaje = 0.25;
                        IconoEstatusActual = "📋";
                        TituloEstatusActual = "Recibido / Pendiente";
                        DescripcionEstatusActual = "Tu orden fue confirmada. El equipo de cocina la revisará en breve.";

                        Fase1BgColor = "#8B5E34"; Fase1TextColor = "#FFFFFF"; Fase1Icono = "✓";
                        Fase2BgColor = "#EAE4DB"; Fase2TextColor = "#A1887F"; Fase2Icono = "2"; Fase2Fecha = "En espera";
                        Fase3BgColor = "#EAE4DB"; Fase3TextColor = "#A1887F"; Fase3Icono = "3"; Fase3Fecha = "En espera";
                        Fase4BgColor = "#EAE4DB"; Fase4TextColor = "#A1887F"; Fase4Icono = "4"; Fase4Fecha = Pedido.FechaEntrega.ToString("dd/MM");
                        break;

                    case 2:
                        ProgresoPorcentaje = 0.50;
                        IconoEstatusActual = "🥣";
                        TituloEstatusActual = "En Preparación / Horneado";
                        DescripcionEstatusActual = "Nuestros maestros reposteros están horneando tu pastel artesanalmente.";

                        Fase1BgColor = "#4CAF50"; Fase1TextColor = "#FFFFFF"; Fase1Icono = "✓";
                        Fase2BgColor = "#8B5E34"; Fase2TextColor = "#FFFFFF"; Fase2Icono = "🥣"; Fase2Fecha = "En proceso";
                        Fase3BgColor = "#EAE4DB"; Fase3TextColor = "#A1887F"; Fase3Icono = "3"; Fase3Fecha = "Siguiente";
                        Fase4BgColor = "#EAE4DB"; Fase4TextColor = "#A1887F"; Fase4Icono = "4"; Fase4Fecha = Pedido.FechaEntrega.ToString("dd/MM");
                        break;

                    case 3:
                        ProgresoPorcentaje = 0.75;
                        IconoEstatusActual = "🎂";
                        TituloEstatusActual = "Decorado / Listo para entrega";
                        DescripcionEstatusActual = "¡El pastel está montado, decorado y empaquetado para su entrega o recolección!";

                        Fase1BgColor = "#4CAF50"; Fase1TextColor = "#FFFFFF"; Fase1Icono = "✓";
                        Fase2BgColor = "#4CAF50"; Fase2TextColor = "#FFFFFF"; Fase2Icono = "✓"; Fase2Fecha = "Completado";
                        Fase3BgColor = "#8B5E34"; Fase3TextColor = "#FFFFFF"; Fase3Icono = "🎂"; Fase3Fecha = "¡Listo!";
                        Fase4BgColor = "#EAE4DB"; Fase4TextColor = "#A1887F"; Fase4Icono = "4"; Fase4Fecha = Pedido.FechaEntrega.ToString("dd/MM");
                        break;

                    case 4:
                        ProgresoPorcentaje = 1.0;
                        IconoEstatusActual = "🚚";
                        TituloEstatusActual = "Entregado con Éxito";
                        DescripcionEstatusActual = "Tu pedido ha sido despachado y entregado. ¡Que lo disfrutes!";

                        Fase1BgColor = "#4CAF50"; Fase1TextColor = "#FFFFFF"; Fase1Icono = "✓";
                        Fase2BgColor = "#4CAF50"; Fase2TextColor = "#FFFFFF"; Fase2Icono = "✓"; Fase2Fecha = "Completado";
                        Fase3BgColor = "#4CAF50"; Fase3TextColor = "#FFFFFF"; Fase3Icono = "✓"; Fase3Fecha = "Completado";
                        Fase4BgColor = "#4CAF50"; Fase4TextColor = "#FFFFFF"; Fase4Icono = "🎉"; Fase4Fecha = Pedido.FechaEntrega.ToString("dd/MM");
                        break;
                }
            }

            OnPropertyChanged(nameof(ProgresoPorcentaje));
            OnPropertyChanged(nameof(IconoEstatusActual));
            OnPropertyChanged(nameof(TituloEstatusActual));
            OnPropertyChanged(nameof(DescripcionEstatusActual));
            OnPropertyChanged(nameof(Fase1BgColor)); OnPropertyChanged(nameof(Fase1TextColor)); OnPropertyChanged(nameof(Fase1Icono)); OnPropertyChanged(nameof(Fase1Fecha));
            OnPropertyChanged(nameof(Fase2BgColor)); OnPropertyChanged(nameof(Fase2TextColor)); OnPropertyChanged(nameof(Fase2Icono)); OnPropertyChanged(nameof(Fase2Fecha));
            OnPropertyChanged(nameof(Fase3BgColor)); OnPropertyChanged(nameof(Fase3TextColor)); OnPropertyChanged(nameof(Fase3Icono)); OnPropertyChanged(nameof(Fase3Fecha));
            OnPropertyChanged(nameof(Fase4BgColor)); OnPropertyChanged(nameof(Fase4TextColor)); OnPropertyChanged(nameof(Fase4Icono)); OnPropertyChanged(nameof(Fase4Fecha));
            OnPropertyChanged(nameof(IsCancelado));
            OnPropertyChanged(nameof(FolioFormateado));
        }

        public async Task RefrescarEstadoEnTiempoReal()
        {
            if (Pedido == null) return;
            var actualizado = await _transaccionesService.GetPedidoByIdAsync(Pedido.Id);
            if (actualizado != null)
            {
                Pedido = actualizado;
                NuevoEstatus = Pedido.Estatus;
                UltimaActualizacionTexto = $"Actualizado en vivo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Sincronización en Tiempo Real", $"Estado actual: {Pedido.Estatus}\nFecha: {DateTime.Now:HH:mm:ss}", "OK");
                }
            }
            else
            {
                UltimaActualizacionTexto = $"Última comprobación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            }
        }

        private async Task ActualizarEstatus()
        {
            if (string.IsNullOrWhiteSpace(NuevoEstatus))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor selecciona un estatus.", "OK");
                return;
            }

            Pedido.Estatus = NuevoEstatus;
            var resultado = await _transaccionesService.ActualizarPedidoAsync(Pedido);

            if (resultado.Exito)
            {
                ActualizarEtapas();
                UltimaActualizacionTexto = $"Actualizado por pastelería: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                await Application.Current.MainPage.DisplayAlert("Éxito", resultado.Mensaje, "OK");
                OnPropertyChanged(nameof(Pedido));
            }
            else
            {
                if (!AppSession.HasAuthError) { await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar el estatus.\nDetalle: {resultado.Mensaje}", "OK"); } AppSession.HasAuthError = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

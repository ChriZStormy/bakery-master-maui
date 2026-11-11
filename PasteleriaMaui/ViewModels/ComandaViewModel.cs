using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    public class ComandaItemViewModel : INotifyPropertyChanged
    {
        private readonly Pedido _pedido;
        public Pedido Pedido => _pedido;

        public ComandaItemViewModel(Pedido pedido)
        {
            _pedido = pedido;
        }

        public int Id => _pedido.Id;
        public string FolioTexto => $"Folio #{_pedido.Id:D4}";
        public string ClienteNombre => !string.IsNullOrWhiteSpace(_pedido.Usuario?.Nombre) 
            ? _pedido.Usuario.Nombre 
            : (!string.IsNullOrWhiteSpace(_pedido.Usuario?.Email) ? _pedido.Usuario.Email : "Cliente General");

        public string PastelNombre => _pedido.Pastel?.Nombre ?? "Pastel Personalizado";
        public string Tamanio => !string.IsNullOrWhiteSpace(_pedido.Pastel?.Tamanio) ? _pedido.Pastel.Tamanio : "Mediano";
        public int Cantidad => _pedido.Cantidad > 0 ? _pedido.Cantidad : 1;

        public string BizcochoNombre => _pedido.Pastel?.Bizcocho?.Nombre ?? "Bizcocho Tradicional";
        public string RellenoNombre => _pedido.Pastel?.Relleno?.Nombre ?? "Sin relleno especificado";
        public string GlaseadoNombre => _pedido.Pastel?.Glaseado?.Nombre ?? "Glaseado Tradicional";

        public string Mensaje => !string.IsNullOrWhiteSpace(_pedido.Mensaje) 
            ? _pedido.Mensaje 
            : (!string.IsNullOrWhiteSpace(_pedido.Pastel?.Mensaje) ? _pedido.Pastel.Mensaje : string.Empty);

        public bool TieneMensaje => !string.IsNullOrWhiteSpace(Mensaje);

        public string? ImagenDecoracion => !string.IsNullOrWhiteSpace(_pedido.ImagenDecoracion) 
            ? _pedido.ImagenDecoracion 
            : _pedido.Pastel?.ImagenDecoracion;

        public bool TieneImagenDecoracion => !string.IsNullOrWhiteSpace(ImagenDecoracion);

        public string Notas
        {
            get => _pedido.Notas ?? string.Empty;
            set
            {
                if (_pedido.Notas != value)
                {
                    _pedido.Notas = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TieneNotas));
                }
            }
        }

        public bool TieneNotas => !string.IsNullOrWhiteSpace(Notas);

        public string FechaHoraTexto => $"{_pedido.FechaEntrega:dd/MM/yyyy} - {(!string.IsNullOrEmpty(_pedido.HoraEntrega) ? _pedido.HoraEntrega : _pedido.FechaEntrega.ToString("HH:mm"))}";

        public string ModalidadTexto => _pedido.ModalidadEntrega == "Domicilio"
            ? $"🚚 A Domicilio: {(!string.IsNullOrWhiteSpace(_pedido.Direccion) ? _pedido.Direccion : "Sin dirección indicada")}"
            : "🏬 Recolección en Pastelería";

        public string Estatus
        {
            get => _pedido.Estatus ?? "Pendiente";
            set
            {
                if (_pedido.Estatus != value)
                {
                    _pedido.Estatus = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BadgeColor));
                    OnPropertyChanged(nameof(BadgeTextColor));
                    OnPropertyChanged(nameof(BotonAccionTexto));
                    OnPropertyChanged(nameof(BotonAccionColor));
                    OnPropertyChanged(nameof(SiguienteEstatus));
                    OnPropertyChanged(nameof(PuedeAvanzar));
                    OnPropertyChanged(nameof(FaseActualIcono));
                }
            }
        }

        public string FaseActualIcono => Estatus switch
        {
            "En Espera" or "Pendiente" => "⏳ En Espera",
            "Horneado" => "🔥 En Horneado",
            "Decoración" => "🎨 En Decoración",
            "Listo" => "✅ Listo para Entrega",
            "Entregado" => "📦 Entregado",
            _ => "⚠️ Cancelado"
        };

        public Color BadgeColor => Estatus switch
        {
            "En Espera" or "Pendiente" => Color.FromArgb("#FFF3E0"),
            "Horneado" => Color.FromArgb("#FBE9E7"),
            "Decoración" => Color.FromArgb("#F3E5F5"),
            "Listo" => Color.FromArgb("#E8F5E9"),
            "Entregado" => Color.FromArgb("#ECEFF1"),
            _ => Color.FromArgb("#FAFAFA")
        };

        public Color BadgeTextColor => Estatus switch
        {
            "En Espera" or "Pendiente" => Color.FromArgb("#E65100"),
            "Horneado" => Color.FromArgb("#BF360C"),
            "Decoración" => Color.FromArgb("#6A1B9A"),
            "Listo" => Color.FromArgb("#2E7D32"),
            "Entregado" => Color.FromArgb("#37474F"),
            _ => Color.FromArgb("#757575")
        };

        public string BotonAccionTexto => Estatus switch
        {
            "En Espera" or "Pendiente" => "🔥 Meter al Horno",
            "Horneado" => "🎨 Pasar a Decoración",
            "Decoración" => "✅ Marcar Listo",
            "Listo" => "📦 Marcar Entregado",
            _ => "Completado"
        };

        public Color BotonAccionColor => Estatus switch
        {
            "En Espera" or "Pendiente" => Color.FromArgb("#D84315"),
            "Horneado" => Color.FromArgb("#7B1FA2"),
            "Decoración" => Color.FromArgb("#2E7D32"),
            "Listo" => Color.FromArgb("#455A64"),
            _ => Color.FromArgb("#9E9E9E")
        };

        public string SiguienteEstatus => Estatus switch
        {
            "En Espera" or "Pendiente" => "Horneado",
            "Horneado" => "Decoración",
            "Decoración" => "Listo",
            "Listo" => "Entregado",
            _ => Estatus
        };

        public bool PuedeAvanzar => Estatus != "Entregado" && Estatus != "Cancelado";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ComandaViewModel : INotifyPropertyChanged, IAsyncDisposable
    {
        private readonly TransaccionesService _transaccionesService;
        private HubConnection? _hubConnection;
        private List<ComandaItemViewModel> _todasLasComandas = new();

        public ObservableCollection<ComandaItemViewModel> ComandasExhibidas { get; } = new();

        // Rol e información
        public bool EsReposteroOAdmin => AppSession.CurrentUser?.Rol == "Repostero" 
                                      || AppSession.CurrentUser?.Rol == "Empleado" 
                                      || AppSession.CurrentUser?.Rol == "Admin";

        public string NombreUsuario => AppSession.CurrentUser?.Nombre ?? "Chef Repostero";

        // Filtro de Fase Activa
        private string _faseFiltro = "Todas";
        public string FaseFiltro
        {
            get => _faseFiltro;
            set
            {
                if (_faseFiltro != value)
                {
                    _faseFiltro = value;
                    OnPropertyChanged();
                    ActualizarEstilosTabs();
                    AplicarFiltro();
                }
            }
        }

        // Estado y Carga
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }

        // Estado SignalR
        private bool _isConectadoRealTime;
        public bool IsConectadoRealTime
        {
            get => _isConectadoRealTime;
            set
            {
                if (_isConectadoRealTime != value)
                {
                    _isConectadoRealTime = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EstadoConexionTexto));
                    OnPropertyChanged(nameof(EstadoConexionColor));
                }
            }
        }

        public string EstadoConexionTexto => IsConectadoRealTime ? "● KDS En Vivo" : "○ KDS Desconectado";
        public Color EstadoConexionColor => IsConectadoRealTime ? Color.FromArgb("#2E7D32") : Color.FromArgb("#BCAAA4");

        // Contadores de Producción
        private int _contadorEnEspera;
        public int ContadorEnEspera
        {
            get => _contadorEnEspera;
            set { _contadorEnEspera = value; OnPropertyChanged(); }
        }

        private int _contadorHorneado;
        public int ContadorHorneado
        {
            get => _contadorHorneado;
            set { _contadorHorneado = value; OnPropertyChanged(); }
        }

        private int _contadorDecoracion;
        public int ContadorDecoracion
        {
            get => _contadorDecoracion;
            set { _contadorDecoracion = value; OnPropertyChanged(); }
        }

        private int _contadorListos;
        public int ContadorListos
        {
            get => _contadorListos;
            set { _contadorListos = value; OnPropertyChanged(); }
        }

        private int _contadorTotalCocina;
        public int ContadorTotalCocina
        {
            get => _contadorTotalCocina;
            set { _contadorTotalCocina = value; OnPropertyChanged(); }
        }

        // Estilos dinámicos de las pestañas
        public Color TabTodasBg => FaseFiltro == "Todas" ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        public Color TabTodasText => FaseFiltro == "Todas" ? Colors.White : Color.FromArgb("#795548");

        public Color TabEsperaBg => FaseFiltro == "En Espera" ? Color.FromArgb("#E65100") : Color.FromArgb("#FAF8F5");
        public Color TabEsperaText => FaseFiltro == "En Espera" ? Colors.White : Color.FromArgb("#795548");

        public Color TabHorneadoBg => FaseFiltro == "Horneado" ? Color.FromArgb("#BF360C") : Color.FromArgb("#FAF8F5");
        public Color TabHorneadoText => FaseFiltro == "Horneado" ? Colors.White : Color.FromArgb("#795548");

        public Color TabDecoracionBg => FaseFiltro == "Decoración" ? Color.FromArgb("#6A1B9A") : Color.FromArgb("#FAF8F5");
        public Color TabDecoracionText => FaseFiltro == "Decoración" ? Colors.White : Color.FromArgb("#795548");

        public Color TabListosBg => FaseFiltro == "Listo" ? Color.FromArgb("#2E7D32") : Color.FromArgb("#FAF8F5");
        public Color TabListosText => FaseFiltro == "Listo" ? Colors.White : Color.FromArgb("#795548");

        // Comandos MVVM
        public ICommand RecargarComandasCommand { get; }
        public ICommand CambiarFiltroCommand { get; }
        public ICommand AvanzarFaseCommand { get; }
        public ICommand CambiarFaseManualCommand { get; }
        public ICommand AgregarNotaCommand { get; }

        public ComandaViewModel(TransaccionesService transaccionesService)
        {
            _transaccionesService = transaccionesService;

            RecargarComandasCommand = new Command(async () => await CargarComandasAsync());
            CambiarFiltroCommand = new Command<string>(filtro => { if (!string.IsNullOrEmpty(filtro)) FaseFiltro = filtro; });
            AvanzarFaseCommand = new Command<ComandaItemViewModel>(async item => await EjecutarAvanzarFaseAsync(item));
            CambiarFaseManualCommand = new Command<ComandaItemViewModel>(async item => await EjecutarCambiarFaseManualAsync(item));
            AgregarNotaCommand = new Command<ComandaItemViewModel>(async item => await EjecutarAgregarNotaAsync(item));
        }

        // Constructor sin parámetros para compatibilidad MAUI XAML
        public ComandaViewModel() : this(new TransaccionesService())
        {
        }

        public async Task InicializarAsync()
        {
            OnPropertyChanged(nameof(EsReposteroOAdmin));
            OnPropertyChanged(nameof(NombreUsuario));

            await CargarComandasAsync();
            await IniciarSignalRAsync();
        }

        public async Task CargarComandasAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            IsRefreshing = true;

            try
            {
                var pedidos = await _transaccionesService.GetPedidosAsync();
                _todasLasComandas = pedidos
                    .OrderBy(p => p.FechaEntrega)
                    .Select(p => new ComandaItemViewModel(p))
                    .ToList();

                ActualizarContadores();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                if (Shell.Current != null)
                {
                    await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar las comandas: {ex.Message}", "OK");
                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private void ActualizarContadores()
        {
            ContadorEnEspera = _todasLasComandas.Count(c => c.Estatus == "En Espera" || c.Estatus == "Pendiente");
            ContadorHorneado = _todasLasComandas.Count(c => c.Estatus == "Horneado");
            ContadorDecoracion = _todasLasComandas.Count(c => c.Estatus == "Decoración");
            ContadorListos = _todasLasComandas.Count(c => c.Estatus == "Listo");
            ContadorTotalCocina = ContadorEnEspera + ContadorHorneado + ContadorDecoracion + ContadorListos;
        }

        private void AplicarFiltro()
        {
            ComandasExhibidas.Clear();

            IEnumerable<ComandaItemViewModel> query = _todasLasComandas;

            switch (FaseFiltro)
            {
                case "En Espera":
                    query = query.Where(c => c.Estatus == "En Espera" || c.Estatus == "Pendiente");
                    break;
                case "Horneado":
                    query = query.Where(c => c.Estatus == "Horneado");
                    break;
                case "Decoración":
                    query = query.Where(c => c.Estatus == "Decoración");
                    break;
                case "Listo":
                    query = query.Where(c => c.Estatus == "Listo");
                    break;
                case "Todas":
                default:
                    // Por defecto, muestra todos los pedidos activos en cocina
                    query = query.Where(c => c.Estatus != "Entregado" && c.Estatus != "Cancelado");
                    break;
            }

            foreach (var item in query)
            {
                ComandasExhibidas.Add(item);
            }
        }

        private void ActualizarEstilosTabs()
        {
            OnPropertyChanged(nameof(TabTodasBg));
            OnPropertyChanged(nameof(TabTodasText));
            OnPropertyChanged(nameof(TabEsperaBg));
            OnPropertyChanged(nameof(TabEsperaText));
            OnPropertyChanged(nameof(TabHorneadoBg));
            OnPropertyChanged(nameof(TabHorneadoText));
            OnPropertyChanged(nameof(TabDecoracionBg));
            OnPropertyChanged(nameof(TabDecoracionText));
            OnPropertyChanged(nameof(TabListosBg));
            OnPropertyChanged(nameof(TabListosText));
        }

        private async Task EjecutarAvanzarFaseAsync(ComandaItemViewModel? item)
        {
            if (item == null || !item.PuedeAvanzar) return;

            var nuevoEstatus = item.SiguienteEstatus;
            var confirmacion = await Shell.Current.DisplayAlert(
                "Avanzar Producción", 
                $"¿Deseas cambiar el Folio #{item.Id:D4} de '{item.Estatus}' a '{nuevoEstatus}'?", 
                "Sí, avanzar", 
                "Cancelar");

            if (!confirmacion) return;

            IsBusy = true;
            try
            {
                var (exito, mensaje) = await _transaccionesService.ActualizarEstatusPedidoAsync(item.Id, nuevoEstatus, item.Notas);
                if (exito)
                {
                    item.Estatus = nuevoEstatus;
                    ActualizarContadores();
                    AplicarFiltro();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", mensaje, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task EjecutarCambiarFaseManualAsync(ComandaItemViewModel? item)
        {
            if (item == null) return;

            var opciones = new[] { "En Espera", "Horneado", "Decoración", "Listo", "Entregado", "Cancelado" };
            var seleccion = await Shell.Current.DisplayActionSheet(
                $"Cambiar Fase - Folio #{item.Id:D4}", 
                "Cancelar", 
                null, 
                opciones);

            if (string.IsNullOrEmpty(seleccion) || seleccion == "Cancelar" || seleccion == item.Estatus)
                return;

            IsBusy = true;
            try
            {
                var (exito, mensaje) = await _transaccionesService.ActualizarEstatusPedidoAsync(item.Id, seleccion, item.Notas);
                if (exito)
                {
                    item.Estatus = seleccion;
                    ActualizarContadores();
                    AplicarFiltro();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", mensaje, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task EjecutarAgregarNotaAsync(ComandaItemViewModel? item)
        {
            if (item == null) return;

            var respuesta = await Shell.Current.DisplayPromptAsync(
                $"Notas Técnicas - Folio #{item.Id:D4}",
                "Escribe instrucciones u observaciones para el equipo de repostería:",
                "Guardar",
                "Cancelar",
                placeholder: "Ej. Pan bien humedecido, entregar en caja alta...",
                initialValue: item.Notas);

            if (respuesta == null) return; // Cancelado

            IsBusy = true;
            try
            {
                var (exito, mensaje) = await _transaccionesService.ActualizarEstatusPedidoAsync(item.Id, item.Estatus, respuesta);
                if (exito)
                {
                    item.Notas = respuesta;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", mensaje, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task IniciarSignalRAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
                return;

#if ANDROID
            var hubUrl = "http://10.0.2.2:5105/hubs/pedidos";
#else
            var hubUrl = "http://127.0.0.1:5105/hubs/pedidos";
#endif

            try
            {
                var handler = MauiProgram.GetHttpClientHandler();

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        options.HttpMessageHandlerFactory = _ => handler;
                    })
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<int, string>("PedidoEstatusActualizado", (pedidoId, nuevoEstatus) =>
                {
                    ActualizarEstatusEnTiempoReal(pedidoId, nuevoEstatus);
                });

                _hubConnection.On<int, string>("EstatusPedidoCambiado", (pedidoId, nuevoEstatus) =>
                {
                    ActualizarEstatusEnTiempoReal(pedidoId, nuevoEstatus);
                });

                _hubConnection.On<int>("NuevoPedidoRegistrado", (pedidoId) =>
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await CargarComandasAsync();
                    });
                });

                _hubConnection.Reconnected += connectionId =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsConectadoRealTime = true;
                    });
                    return Task.CompletedTask;
                };

                _hubConnection.Closed += error =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsConectadoRealTime = false;
                    });
                    return Task.CompletedTask;
                };

                await _hubConnection.StartAsync();
                await _hubConnection.InvokeAsync("SuscribirAPedidos");
                IsConectadoRealTime = true;
            }
            catch
            {
                IsConectadoRealTime = false;
            }
        }

        private void ActualizarEstatusEnTiempoReal(int pedidoId, string nuevoEstatus)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var item = _todasLasComandas.FirstOrDefault(c => c.Id == pedidoId);
                if (item != null)
                {
                    item.Estatus = nuevoEstatus;
                    ActualizarContadores();
                    AplicarFiltro();
                }
                else
                {
                    // Si no estaba en memoria, recargar comandas completas
                    _ = CargarComandasAsync();
                }
            });
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                try
                {
                    await _hubConnection.StopAsync();
                    await _hubConnection.DisposeAsync();
                }
                catch { }
                _hubConnection = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

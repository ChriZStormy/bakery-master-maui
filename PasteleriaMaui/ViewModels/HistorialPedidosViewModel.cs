using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;
using PasteleriaMaui.Views;

namespace PasteleriaMaui.ViewModels
{
    public class HistorialPedidosViewModel : INotifyPropertyChanged, IAsyncDisposable
    {
        private readonly TransaccionesService _transaccionesService;
        private HubConnection? _hubConnection;
        private List<Pedido> _todosLosPedidos = new();

        public ObservableCollection<Pedido> Pedidos { get; set; } = new();

        public bool EsCliente => AppSession.CurrentUser?.Rol == "Cliente";
        public bool EsAdmin => AppSession.CurrentUser?.Rol == "Admin";
        public string TituloPagina => EsCliente ? "Mis Pedidos" : "Historial de Pedidos";
        public string LeyendaTexto => EsCliente ? "Pedidos que has realizado" : "Gestión General de Pedidos";

        private string _currentMainTab = "En Proceso";
        public string CurrentMainTab
        {
            get => _currentMainTab;
            set
            {
                if (_currentMainTab != value)
                {
                    _currentMainTab = value;
                    OnPropertyChanged();
                    ActualizarEstilosTabs();
                    FiltrarPedidosAdmin();
                }
            }
        }

        private string _currentSubTab = "Pasteles";
        public string CurrentSubTab
        {
            get => _currentSubTab;
            set
            {
                if (_currentSubTab != value)
                {
                    _currentSubTab = value;
                    OnPropertyChanged();
                    ActualizarEstilosTabs();
                    FiltrarPedidosAdmin();
                }
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }

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

        public string EstadoConexionTexto => IsConectadoRealTime ? "● En vivo" : "○ Desconectado";
        public Color EstadoConexionColor => IsConectadoRealTime ? Color.FromArgb("#2E7D32") : Color.FromArgb("#BCAAA4");

        // Colores dinámicos para pestañas de Administrador
        public Color TabEnProcesoBg => CurrentMainTab == "En Proceso" ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        public Color TabEnProcesoText => CurrentMainTab == "En Proceso" ? Colors.White : Color.FromArgb("#795548");

        public Color TabEntregadosBg => CurrentMainTab == "Entregados" ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        public Color TabEntregadosText => CurrentMainTab == "Entregados" ? Colors.White : Color.FromArgb("#795548");

        public Color TabCanceladosBg => CurrentMainTab == "Cancelados" ? Color.FromArgb("#8B5E34") : Color.FromArgb("#FAF8F5");
        public Color TabCanceladosText => CurrentMainTab == "Cancelados" ? Colors.White : Color.FromArgb("#795548");

        public Color SubTabPastelesBg => CurrentSubTab == "Pasteles" ? Color.FromArgb("#5D4037") : Color.FromArgb("#F5F0E6");
        public Color SubTabPastelesText => CurrentSubTab == "Pasteles" ? Colors.White : Color.FromArgb("#795548");

        public Color SubTabOtrosBg => CurrentSubTab == "Otros" ? Color.FromArgb("#5D4037") : Color.FromArgb("#F5F0E6");
        public Color SubTabOtrosText => CurrentSubTab == "Otros" ? Colors.White : Color.FromArgb("#795548");

        private Pedido? _pedidoSeleccionado;
        public Pedido? PedidoSeleccionado
        {
            get => _pedidoSeleccionado;
            set
            {
                if (_pedidoSeleccionado != value)
                {
                    _pedidoSeleccionado = value;
                    OnPropertyChanged();
                    if (value != null)
                    {
                        var pedidoParaNavegar = value;
                        _pedidoSeleccionado = null;
                        OnPropertyChanged();
                        _ = NavegarADetallePedido(pedidoParaNavegar);
                    }
                }
            }
        }

        // Comandos MVVM
        public ICommand RealizarPedidoPersonalizadoCommand { get; }
        public ICommand VerCalendarioCommand { get; }
        public ICommand RecargarPedidosCommand { get; }
        public ICommand CambiarTabPrincipalCommand { get; }
        public ICommand CambiarSubTabCommand { get; }

        public HistorialPedidosViewModel()
        {
            _transaccionesService = new TransaccionesService();

            RealizarPedidoPersonalizadoCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(PedidoPersonalizadoPage)));
            VerCalendarioCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(CalendarioPedidosPage)));
            RecargarPedidosCommand = new Command(async () => await CargarPedidosAsync());
            CambiarTabPrincipalCommand = new Command<string>(tab => { if (!string.IsNullOrEmpty(tab)) CurrentMainTab = tab; });
            CambiarSubTabCommand = new Command<string>(subTab => { if (!string.IsNullOrEmpty(subTab)) CurrentSubTab = subTab; });
        }

        public async Task InicializarAsync()
        {
            OnPropertyChanged(nameof(EsCliente));
            OnPropertyChanged(nameof(EsAdmin));
            OnPropertyChanged(nameof(TituloPagina));
            OnPropertyChanged(nameof(LeyendaTexto));

            await CargarPedidosAsync();
            await IniciarSignalRAsync();
        }

        public async Task CargarPedidosAsync()
        {
            try
            {
                IsRefreshing = true;

                string? filterId = null;
                if (AppSession.CurrentUser != null && AppSession.CurrentUser.Rol == "Cliente")
                {
                    filterId = AppSession.CurrentUser.Id;
                }

                var lista = await _transaccionesService.GetPedidosAsync(filterId);
                _todosLosPedidos = lista.OrderByDescending(p => p.Fecha).ToList();

                if (EsAdmin)
                {
                    FiltrarPedidosAdmin();
                }
                else
                {
                    Pedidos.Clear();
                    foreach (var p in _todosLosPedidos)
                    {
                        Pedidos.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HistorialPedidos] Error al cargar pedidos: {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public void FiltrarPedidosAdmin()
        {
            var filtrados = _todosLosPedidos.AsEnumerable();

            // Filtro por pestaña principal
            if (CurrentMainTab == "En Proceso")
            {
                filtrados = filtrados.Where(p => p.Estatus == "Pendiente" || p.Estatus == "En Proceso");
            }
            else if (CurrentMainTab == "Entregados")
            {
                filtrados = filtrados.Where(p => p.Estatus == "Entregado" || p.Estatus == "Entregados");
            }
            else if (CurrentMainTab == "Cancelados")
            {
                filtrados = filtrados.Where(p => p.Estatus == "Cancelado" || p.Estatus == "Cancelados");
            }

            // Filtro por sub-pestaña
            if (CurrentSubTab == "Pasteles")
            {
                filtrados = filtrados.Where(p => p.Pastel != null);
            }
            else if (CurrentSubTab == "Otros")
            {
                filtrados = Enumerable.Empty<Pedido>();
            }

            var ordenados = filtrados.OrderByDescending(p => p.Fecha).ToList();

            Pedidos.Clear();
            foreach (var p in ordenados)
            {
                Pedidos.Add(p);
            }
        }

        private void ActualizarEstilosTabs()
        {
            OnPropertyChanged(nameof(TabEnProcesoBg));
            OnPropertyChanged(nameof(TabEnProcesoText));
            OnPropertyChanged(nameof(TabEntregadosBg));
            OnPropertyChanged(nameof(TabEntregadosText));
            OnPropertyChanged(nameof(TabCanceladosBg));
            OnPropertyChanged(nameof(TabCanceladosText));

            OnPropertyChanged(nameof(SubTabPastelesBg));
            OnPropertyChanged(nameof(SubTabPastelesText));
            OnPropertyChanged(nameof(SubTabOtrosBg));
            OnPropertyChanged(nameof(SubTabOtrosText));
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
                        await CargarPedidosAsync();
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
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR Client] Error al conectar: {ex.Message}");
                IsConectadoRealTime = false;
            }
        }

        private void ActualizarEstatusEnTiempoReal(int pedidoId, string nuevoEstatus)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Actualizar en lista maestra
                var enLista = _todosLosPedidos.FirstOrDefault(p => p.Id == pedidoId);
                if (enLista != null)
                {
                    enLista.Estatus = nuevoEstatus;
                }

                // Actualizar en ObservableCollection visible
                var visible = Pedidos.FirstOrDefault(p => p.Id == pedidoId);
                if (visible != null)
                {
                    visible.Estatus = nuevoEstatus;
                    int idx = Pedidos.IndexOf(visible);
                    if (idx >= 0)
                    {
                        // Disparar re-render de la fila en la CollectionView
                        Pedidos[idx] = visible;
                    }
                }

                if (EsAdmin)
                {
                    FiltrarPedidosAdmin();
                }
            });
        }

        private async Task NavegarADetallePedido(Pedido pedido)
        {
            await Shell.Current.Navigation.PushAsync(new DetallePedidoPage(pedido));
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
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

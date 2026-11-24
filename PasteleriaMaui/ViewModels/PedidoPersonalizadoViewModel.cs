using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    [QueryProperty(nameof(PastelBase), "PastelBase")]
    public class PedidoPersonalizadoViewModel : INotifyPropertyChanged
    {
        private readonly PastelApiService _pastelService;
        private readonly TransaccionesService _transaccionesService;

        private Pastel? _pastelBase;
        public Pastel? PastelBase
        {
            get => _pastelBase;
            set
            {
                _pastelBase = value;
                if (_pastelBase != null)
                {
                    Nombre = _pastelBase.Nombre;
                    if (!string.IsNullOrWhiteSpace(_pastelBase.Tamanio)) Tamanio = _pastelBase.Tamanio;
                    _ = AplicarPastelBaseAsync();
                }
                OnPropertyChanged();
            }
        }

        private async Task AplicarPastelBaseAsync()
        {
            if (_pastelBase == null) return;
            if (_pastelBase.BizcochoId == 0 && _pastelBase.Id > 0)
            {
                var completo = await _pastelService.GetPastelByIdAsync(_pastelBase.Id);
                if (completo != null) _pastelBase = completo;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Bizcochos.Any())
                {
                    var bSel = Bizcochos.FirstOrDefault(b => b.Id == _pastelBase.BizcochoId || b.Nombre == _pastelBase.Bizcocho?.Nombre);
                    if (bSel != null) { BizcochoSeleccionado = null; BizcochoSeleccionado = bSel; }
                }
                if (Rellenos.Any())
                {
                    var rSel = Rellenos.FirstOrDefault(r => r.Id == _pastelBase.RellenoId || r.Nombre == _pastelBase.Relleno?.Nombre);
                    if (rSel != null) { RellenoSeleccionado = null; RellenoSeleccionado = rSel; }
                }
                if (Glaseados.Any())
                {
                    var gSel = Glaseados.FirstOrDefault(g => g.Id == _pastelBase.GlaseadoId || g.Nombre == _pastelBase.Glaseado?.Nombre);
                    if (gSel != null) { GlaseadoSeleccionado = null; GlaseadoSeleccionado = gSel; }
                }
                if (Categorias.Any())
                {
                    var cSel = Categorias.FirstOrDefault(c => c.Id == _pastelBase.CategoriaId);
                    if (cSel != null) CategoriaSeleccionada = cSel;
                }
                OnPropertyChanged(nameof(ResumenSeleccion));
            });
        }

        public ObservableCollection<CatBizcocho> Bizcochos { get; set; } = new();
        public ObservableCollection<CatRelleno> Rellenos { get; set; } = new();
        public ObservableCollection<CatGlaseado> Glaseados { get; set; } = new();
        public ObservableCollection<CatCategoria> Categorias { get; set; } = new();

        private string _nombre = "Mi Pastel Soñado";
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _tamanio = "Mediano";
        public string Tamanio
        {
            get => _tamanio;
            set { _tamanio = value; OnPropertyChanged(); }
        }

        private string _mensaje = string.Empty;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                var val = value ?? string.Empty;
                if (val.Length > 50) val = val.Substring(0, 50);
                _mensaje = val;
                OnPropertyChanged();
            }
        }

        private string? _imagenDecoracion;
        public string? ImagenDecoracion
        {
            get => _imagenDecoracion;
            set
            {
                _imagenDecoracion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TieneDecoracion));
            }
        }

        private string _nombreArchivoDecoracion = "Sin foto de decoración";
        public string NombreArchivoDecoracion
        {
            get => _nombreArchivoDecoracion;
            set { _nombreArchivoDecoracion = value; OnPropertyChanged(); }
        }

        public bool TieneDecoracion => !string.IsNullOrEmpty(ImagenDecoracion);

        public string FotoUrl { get; set; } = string.Empty;

        private DateTime _fechaEntrega = DateTime.Today.AddDays(2);
        public DateTime FechaEntrega
        {
            get => _fechaEntrega;
            set { _fechaEntrega = value; OnPropertyChanged(); }
        }

        public DateTime FechaMinima => DateTime.Today.AddDays(2);

        private string _cantidad = "1";
        public string Cantidad
        {
            get => _cantidad;
            set { _cantidad = value; OnPropertyChanged(); }
        }

        private CatBizcocho? _bizcochoSeleccionado;
        public CatBizcocho? BizcochoSeleccionado
        {
            get => _bizcochoSeleccionado;
            set
            {
                if (_bizcochoSeleccionado != value)
                {
                    _bizcochoSeleccionado = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BizcochoPreviewUrl));
                    OnPropertyChanged(nameof(ResumenSeleccion));
                }
            }
        }

        private CatRelleno? _rellenoSeleccionado;
        public CatRelleno? RellenoSeleccionado
        {
            get => _rellenoSeleccionado;
            set
            {
                if (_rellenoSeleccionado != value)
                {
                    _rellenoSeleccionado = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RellenoPreviewUrl));
                    OnPropertyChanged(nameof(ResumenSeleccion));
                }
            }
        }

        private CatGlaseado? _glaseadoSeleccionado;
        public CatGlaseado? GlaseadoSeleccionado
        {
            get => _glaseadoSeleccionado;
            set
            {
                if (_glaseadoSeleccionado != value)
                {
                    _glaseadoSeleccionado = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(GlaseadoPreviewUrl));
                    OnPropertyChanged(nameof(ResumenSeleccion));
                }
            }
        }

        private CatCategoria? _categoriaSeleccionada;
        public CatCategoria? CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                if (_categoriaSeleccionada != value)
                {
                    _categoriaSeleccionada = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BizcochoPreviewUrl => BizcochoSeleccionado?.ImagenUrl ?? "bizcocho_vainilla.png";
        public string RellenoPreviewUrl => RellenoSeleccionado?.ImagenUrl ?? "relleno_fresa.png";
        public string GlaseadoPreviewUrl => GlaseadoSeleccionado?.ImagenUrl ?? "glaseado_fondant.png";

        public string ResumenSeleccion =>
            $"Pan: {BizcochoSeleccionado?.Nombre ?? "---"} (+${BizcochoSeleccionado?.CostoAdicional ?? 0m:N2}) • Relleno: {RellenoSeleccionado?.Nombre ?? "---"} (+${RellenoSeleccionado?.CostoAdicional ?? 0m:N2}) • Cubierta: {GlaseadoSeleccionado?.Nombre ?? "---"} (+${GlaseadoSeleccionado?.CostoAdicional ?? 0m:N2})";

        public decimal TotalEstimado
        {
            get
            {
                decimal baseTamanio = Tamanio switch
                {
                    "Chico" => 350m,
                    "Grande" => 650m,
                    "Extra Grande" => 850m,
                    _ => 480m
                };
                return baseTamanio + (BizcochoSeleccionado?.CostoAdicional ?? 0m) + (RellenoSeleccionado?.CostoAdicional ?? 0m) + (GlaseadoSeleccionado?.CostoAdicional ?? 0m);
            }
        }

        public string TotalEstimadoTexto => $"Total estimado: ${TotalEstimado:N2} MXN";

        public ICommand GuardarPedidoCommand { get; }
        public ICommand SeleccionarDecoracionCommand { get; }
        public ICommand SeleccionarBizcochoCommand { get; }
        public ICommand SeleccionarRellenoCommand { get; }
        public ICommand SeleccionarGlaseadoCommand { get; }

        public PedidoPersonalizadoViewModel()
        {
            _pastelService = new PastelApiService();
            _transaccionesService = new TransaccionesService();

            GuardarPedidoCommand = new Command(async () => await GuardarPedido());
            SeleccionarDecoracionCommand = new Command(async () => await SeleccionarDecoracion());
            SeleccionarBizcochoCommand = new Command<CatBizcocho>(b => { if (b != null) BizcochoSeleccionado = b; });
            SeleccionarRellenoCommand = new Command<CatRelleno>(r => { if (r != null) RellenoSeleccionado = r; });
            SeleccionarGlaseadoCommand = new Command<CatGlaseado>(g => { if (g != null) GlaseadoSeleccionado = g; });

            _ = CargarCatalogosAsync();
        }

        public async Task CargarCatalogosAsync()
        {
            try
            {
                var bizcos = await _pastelService.GetBizcochosAsync();
                if (bizcos.Count > 0)
                {
                    Bizcochos.Clear();
                    foreach (var b in bizcos) Bizcochos.Add(b);
                    BizcochoSeleccionado = _pastelBase != null
                        ? (Bizcochos.FirstOrDefault(b => b.Id == _pastelBase.BizcochoId || b.Nombre == _pastelBase.Bizcocho?.Nombre) ?? Bizcochos[0])
                        : (BizcochoSeleccionado ?? Bizcochos[0]);
                }

                var rels = await _pastelService.GetRellenosAsync();
                if (rels.Count > 0)
                {
                    Rellenos.Clear();
                    foreach (var r in rels) Rellenos.Add(r);
                    RellenoSeleccionado = _pastelBase != null
                        ? (Rellenos.FirstOrDefault(r => r.Id == _pastelBase.RellenoId || r.Nombre == _pastelBase.Relleno?.Nombre) ?? Rellenos[0])
                        : (RellenoSeleccionado ?? Rellenos[0]);
                }

                var glas = await _pastelService.GetGlaseadosAsync();
                if (glas.Count > 0)
                {
                    Glaseados.Clear();
                    foreach (var g in glas) Glaseados.Add(g);
                    GlaseadoSeleccionado = _pastelBase != null
                        ? (Glaseados.FirstOrDefault(g => g.Id == _pastelBase.GlaseadoId || g.Nombre == _pastelBase.Glaseado?.Nombre) ?? Glaseados[0])
                        : (GlaseadoSeleccionado ?? Glaseados[0]);
                }

                var cats = await _pastelService.GetCategoriasAsync();
                Categorias.Clear();
                foreach (var c in cats) Categorias.Add(c);
                if (_pastelBase != null && Categorias.Count > 0)
                    CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.Id == _pastelBase.CategoriaId) ?? Categorias[0];

                await Task.Delay(150);
                if (_pastelBase != null && _pastelBase.BizcochoId == 0 && _pastelBase.Id > 0)
                {
                    var completo = await _pastelService.GetPastelByIdAsync(_pastelBase.Id);
                    if (completo != null) _pastelBase = completo;
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (_pastelBase != null)
                    {
                        var bSel = Bizcochos.FirstOrDefault(b => b.Id == _pastelBase.BizcochoId || b.Nombre == _pastelBase.Bizcocho?.Nombre);
                        var rSel = Rellenos.FirstOrDefault(r => r.Id == _pastelBase.RellenoId || r.Nombre == _pastelBase.Relleno?.Nombre);
                        var gSel = Glaseados.FirstOrDefault(g => g.Id == _pastelBase.GlaseadoId || g.Nombre == _pastelBase.Glaseado?.Nombre);
                        if (bSel != null) { BizcochoSeleccionado = null; BizcochoSeleccionado = bSel; }
                        if (rSel != null) { RellenoSeleccionado = null; RellenoSeleccionado = rSel; }
                        if (gSel != null) { GlaseadoSeleccionado = null; GlaseadoSeleccionado = gSel; }
                    }
                    OnPropertyChanged(nameof(BizcochoSeleccionado));
                    OnPropertyChanged(nameof(RellenoSeleccionado));
                    OnPropertyChanged(nameof(GlaseadoSeleccionado));
                    OnPropertyChanged(nameof(CategoriaSeleccionada));
                    OnPropertyChanged(nameof(ResumenSeleccion));
                });

                // Fallback de contingencia: Si la API de categorías no devuelve datos, asegurar catálogo base
                if (Categorias.Count == 0)
                {
                    var catsBase = new List<CatCategoria>
                    {
                        new() { Id = 1, categoria = "Gourmet" },
                        new() { Id = 2, categoria = "Infantil" },
                        new() { Id = 3, categoria = "Boda" },
                        new() { Id = 4, categoria = "Tradicional" },
                        new() { Id = 5, categoria = "Dietético" },
                        new() { Id = 6, categoria = "Quinceañera" },
                        new() { Id = 7, categoria = "Temático" },
                        new() { Id = 8, categoria = "Vegano" },
                        new() { Id = 9, categoria = "Aniversario" }
                    };
                    foreach (var c in catsBase) Categorias.Add(c);
                }

                if (Categorias.Count > 0 && CategoriaSeleccionada == null)
                {
                    CategoriaSeleccionada = Categorias[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar catálogos: {ex.Message}");
                if (Categorias.Count == 0)
                {
                    var catsBase = new List<CatCategoria>
                    {
                        new() { Id = 1, categoria = "Gourmet" },
                        new() { Id = 2, categoria = "Infantil" },
                        new() { Id = 3, categoria = "Boda" },
                        new() { Id = 4, categoria = "Tradicional" },
                        new() { Id = 5, categoria = "Temático" }
                    };
                    foreach (var c in catsBase) Categorias.Add(c);
                    CategoriaSeleccionada = Categorias[0];
                }
            }
        }

        private async Task SeleccionarDecoracion()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona la foto de referencia para decoración",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    ImagenDecoracion = result.FullPath;
                    NombreArchivoDecoracion = result.FileName;
                    FotoUrl = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cargar la imagen: {ex.Message}", "OK");
            }
        }

        private async Task GuardarPedido()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                BizcochoSeleccionado == null ||
                RellenoSeleccionado == null ||
                GlaseadoSeleccionado == null ||
                CategoriaSeleccionada == null ||
                string.IsNullOrWhiteSpace(Tamanio) ||
                string.IsNullOrWhiteSpace(Cantidad))
            {
                await Application.Current.MainPage.DisplayAlert("Campos Incompletos", "Por favor completa la información básica de tu pastel.", "OK");
                return;
            }

            if (!int.TryParse(Cantidad, out int qty) || qty <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Cantidad Inválida", "La cantidad debe ser un número entero mayor a 0.", "OK");
                return;
            }

            var nuevoPastel = new Pastel
            {
                Nombre = Nombre,
                Tamanio = Tamanio,
                FotoUrl = FotoUrl,
                Mensaje = Mensaje,
                ImagenDecoracion = ImagenDecoracion,
                BizcochoId = BizcochoSeleccionado.Id,
                RellenoId = RellenoSeleccionado.Id,
                GlaseadoId = GlaseadoSeleccionado.Id,
                CategoriaId = CategoriaSeleccionada.Id > 0 ? CategoriaSeleccionada.Id : 1,
                IsPersonalizado = true,
                Activo = true
            };

            var pastelCreado = await _pastelService.RegistrarPastelAsync(nuevoPastel);
            if (pastelCreado != null)
            {
                var nuevoPedido = new Pedido
                {
                    PastelId = pastelCreado.Id,
                    UsuarioId = AppSession.CurrentUser?.Id,
                    Cantidad = qty,
                    FechaEntrega = FechaEntrega,
                    Mensaje = Mensaje,
                    ImagenDecoracion = ImagenDecoracion,
                    HoraEntrega = "12:00 PM",
                    ModalidadEntrega = "Recoger en Tienda"
                };

                var exito = await _transaccionesService.CrearPedidoAsync(nuevoPedido);
                if (exito)
                {
                    await Application.Current.MainPage.DisplayAlert("¡Pedido Exitoso!", "Tu pastel personalizado ha sido configurado y tu pedido se generó correctamente.", "Aceptar");
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error al Generar Pedido", "El pastel personalizado se guardó, pero hubo un problema al generar la orden de pedido.", "OK");
                }
            }
            else
            {
                if (!AppSession.HasAuthError)
                {
                    var detalle = !string.IsNullOrWhiteSpace(_pastelService.UltimoError)
                        ? $"\n\nDetalle: {_pastelService.UltimoError}"
                        : "";
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo registrar la creación del pastel en el servidor.{detalle}", "OK");
                }
                AppSession.HasAuthError = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.ViewModels
{
    public class GestionUsuariosViewModel : INotifyPropertyChanged
    {
        private readonly IAdminService _adminService;
        private bool _isRefreshing;

        public ObservableCollection<UsuarioItemViewModel> Usuarios { get; } = new();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }

        public ICommand CargarUsuariosCommand { get; }
        public ICommand AsignarRolCommand { get; }
        public ICommand ToggleUserStatusCommand { get; }
        public ICommand EditarUsuarioCommand { get; }

        public GestionUsuariosViewModel() : this(new AdminService())
        {
        }

        public GestionUsuariosViewModel(IAdminService adminService)
        {
            _adminService = adminService;
            CargarUsuariosCommand = new Command(async () => await CargarUsuariosAsync());
            AsignarRolCommand = new Command<UsuarioItemViewModel>(async (item) => await AsignarRolAsync(item));
            ToggleUserStatusCommand = new Command<UsuarioItemViewModel>(async (item) => await ToggleUserStatusAsync(item));
            EditarUsuarioCommand = new Command<UsuarioItemViewModel>(async (item) => await EditarUsuarioAsync(item));
        }

        public async Task CargarUsuariosAsync()
        {
            try
            {
                IsRefreshing = true;
                var users = await _adminService.GetUsersAsync();
                Usuarios.Clear();

                foreach (var user in users)
                {
                    Usuarios.Add(new UsuarioItemViewModel
                    {
                        Id = user.Id,
                        Nombre = user.Nombre,
                        Email = user.Email,
                        Rol = user.Rol,
                        RolSeleccionado = user.Rol,
                        Activo = user.Activo
                    });
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cargar la lista de usuarios: {ex.Message}", "OK");
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task AsignarRolAsync(UsuarioItemViewModel? item)
        {
            if (item == null) return;

            if (item.IsCurrentUser)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Acción Denegada", "No puedes modificar tu propio rol de administrador.", "OK");
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(item.RolSeleccionado))
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Por favor selecciona un rol.", "OK");
                }
                return;
            }

            if (string.Equals(item.Rol, item.RolSeleccionado, StringComparison.OrdinalIgnoreCase))
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Sin cambios", $"El usuario '{item.Nombre}' ya cuenta con el rol '{item.Rol}'.", "OK");
                }
                return;
            }

            bool confirmar = false;
            if (Application.Current?.MainPage != null)
            {
                confirmar = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar Asignación de Rol",
                    $"¿Deseas cambiar el rol de '{item.Nombre}' de '{item.Rol}' a '{item.RolSeleccionado}'?",
                    "Sí, asignar",
                    "Cancelar");
            }

            if (!confirmar)
            {
                item.RolSeleccionado = item.Rol;
                return;
            }

            var (exito, mensaje) = await _adminService.AssignRoleAsync(item.Id, item.RolSeleccionado);
            if (exito)
            {
                item.Rol = item.RolSeleccionado;
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");
                }
                await CargarUsuariosAsync();
            }
            else
            {
                item.RolSeleccionado = item.Rol;
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", mensaje, "OK");
                }
            }
        }

        private async Task ToggleUserStatusAsync(UsuarioItemViewModel? item)
        {
            if (item == null) return;

            if (item.IsCurrentUser)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Acción Denegada", "No puedes dar de baja o suspender tu propia cuenta de Administrador.", "OK");
                }
                return;
            }

            string accion = item.Activo ? "dar de baja temporalmente" : "reactivar";
            bool confirmar = false;
            if (Application.Current?.MainPage != null)
            {
                confirmar = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar Estado de Usuario",
                    $"¿Estás seguro de {accion} la cuenta de '{item.Nombre}'?",
                    "Sí, confirmar",
                    "Cancelar");
            }

            if (!confirmar) return;

            var (exito, mensaje) = await _adminService.ToggleUserStatusAsync(item.Id);
            if (exito)
            {
                item.Activo = !item.Activo;
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");
                }
            }
            else
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", mensaje, "OK");
                }
            }
        }

        private async Task EditarUsuarioAsync(UsuarioItemViewModel? item)
        {
            if (item == null) return;

            if (Application.Current?.MainPage == null) return;

            string nuevoNombre = await Application.Current.MainPage.DisplayPromptAsync(
                "Modificar Usuario",
                "Ingresa el nuevo nombre:",
                initialValue: item.Nombre,
                placeholder: "Nombre completo");

            if (string.IsNullOrWhiteSpace(nuevoNombre)) return;

            string nuevoEmail = await Application.Current.MainPage.DisplayPromptAsync(
                "Modificar Usuario",
                "Ingresa el nuevo correo electrónico:",
                initialValue: item.Email,
                placeholder: "correo@ejemplo.com",
                keyboard: Keyboard.Email);

            if (string.IsNullOrWhiteSpace(nuevoEmail)) return;

            var (exito, mensaje) = await _adminService.UpdateUserAsync(item.Id, nuevoNombre, nuevoEmail);
            if (exito)
            {
                item.Nombre = nuevoNombre;
                item.Email = nuevoEmail;
                await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");
                await CargarUsuariosAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", mensaje, "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UsuarioItemViewModel : INotifyPropertyChanged
    {
        private string _rol = string.Empty;
        private string _rolSeleccionado = string.Empty;
        private string _nombre = string.Empty;
        private string _email = string.Empty;
        private bool _activo = true;

        public string Id { get; set; } = string.Empty;

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public bool Activo
        {
            get => _activo;
            set
            {
                _activo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
                OnPropertyChanged(nameof(StatusBadgeBgColor));
                OnPropertyChanged(nameof(StatusBadgeTextColor));
                OnPropertyChanged(nameof(ToggleStatusButtonText));
                OnPropertyChanged(nameof(ToggleStatusButtonBgColor));
                OnPropertyChanged(nameof(ToggleStatusButtonTextColor));
            }
        }

        public string Rol
        {
            get => _rol;
            set
            {
                _rol = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsRepostero));
                OnPropertyChanged(nameof(RoleBadgeBgColor));
                OnPropertyChanged(nameof(RoleBadgeTextColor));
            }
        }

        public string RolSeleccionado
        {
            get => _rolSeleccionado;
            set
            {
                _rolSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public static List<string> RolesList { get; } = new() { "Cliente", "Repostero", "Admin" };
        public List<string> RolesDisponibles => RolesList;

        public bool IsAdmin => string.Equals(Rol, "Admin", StringComparison.OrdinalIgnoreCase);
        public bool IsRepostero => string.Equals(Rol, "Repostero", StringComparison.OrdinalIgnoreCase);
        public bool IsCurrentUser => Id == AppSession.CurrentUser?.Id;
        public bool CanEditRole => !IsCurrentUser;

        // Estado del usuario (Baja Lógica vs Activo)
        public string StatusText => Activo ? "Activo" : "Suspendido";
        public Color StatusBadgeBgColor => Activo ? Color.FromArgb("#E8F5E9") : Color.FromArgb("#FFEBEE");
        public Color StatusBadgeTextColor => Activo ? Color.FromArgb("#2E7D32") : Color.FromArgb("#C62828");

        public string ToggleStatusButtonText => Activo ? "Dar de baja" : "Reactivar";
        public Color ToggleStatusButtonBgColor => Activo ? Color.FromArgb("#FFF0F0") : Color.FromArgb("#E8F5E9");
        public Color ToggleStatusButtonTextColor => Activo ? Color.FromArgb("#D32F2F") : Color.FromArgb("#2E7D32");

        public Color RoleBadgeBgColor
        {
            get
            {
                if (IsAdmin) return Color.FromArgb("#FFF3E0"); // Naranja pastel
                if (IsRepostero) return Color.FromArgb("#E0F2F1"); // Verde azulado pastel
                return Color.FromArgb("#EFEBE9"); // Gris cálido pastel
            }
        }

        public Color RoleBadgeTextColor
        {
            get
            {
                if (IsAdmin) return Color.FromArgb("#E65100");
                if (IsRepostero) return Color.FromArgb("#00695C");
                return Color.FromArgb("#4E342E");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

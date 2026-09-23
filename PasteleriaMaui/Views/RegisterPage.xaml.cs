using PasteleriaMaui.Models;
using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views
{
    public partial class RegisterPage : ContentPage
    {
        private readonly AuthService _authService;

        public RegisterPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }
        

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || RolPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Llena todos los campos", "OK");
                return;
            }


            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            var passRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            
            if (!emailRegex.IsMatch(EmailEntry.Text))
            {
                await DisplayAlert("Error", "Formato de correo inválido", "OK");
                return;
            }
            if (!passRegex.IsMatch(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial", "OK");
                return;
            }


            var nuevoUsuario = new Usuario
            {
                Nombre = NombreEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text,
                Rol = RolPicker.SelectedItem.ToString()
            };


            var user = await _authService.RegisterAsync(nuevoUsuario);
            if (user != null)
            {
                await DisplayAlert("Éxito", "Usuario creado. Ahora inicia sesión.", "OK");
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar (quizás el correo ya existe)", "OK");
            }
        }


        private void OnBackClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }


        private void OnTogglePasswordVisibilityClicked(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
            if (sender is Button button)
            {
                button.Text = PasswordEntry.IsPassword ? "◉" : "◠";
            }
        }
    }
}

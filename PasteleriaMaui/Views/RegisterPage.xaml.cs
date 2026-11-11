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
            var nombre = NombreEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Llena todos los campos", "OK");
                return;
            }

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            var passRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            
            if (!emailRegex.IsMatch(email))
            {
                await DisplayAlert("Error", "Formato de correo inválido", "OK");
                return;
            }
            if (!passRegex.IsMatch(password))
            {
                await DisplayAlert("Error", "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial", "OK");
                return;
            }

            var user = await _authService.RegisterAsync(nombre, email, password);
            if (user != null)
            {
                AppSession.CurrentUser = user;
                Application.Current.MainPage = new AppShell();
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

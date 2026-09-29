using PasteleriaMaui.Services;

namespace PasteleriaMaui.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _authService;

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Ingresa tus credenciales", "OK");
                return;
            }

            try
            {
                var user = await _authService.LoginAsync(email, password);
                if (user != null)
                {
                    AppSession.CurrentUser = user;
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await DisplayAlert("Error", "Credenciales incorrectas", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de Conexión", "No se pudo conectar al servidor de la API. Asegúrate de que PasteleriaAPI esté corriendo. Detalle: " + ex.Message, "OK");
            }
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
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

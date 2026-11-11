namespace PasteleriaMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Try to restore session from SecureStorage
            var token = SecureStorage.GetAsync("jwt_token").GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(token) && !Helpers.JwtClaimsParser.IsTokenExpired(token))
            {
                var usuario = Helpers.JwtClaimsParser.ParseToken(token);
                if (usuario != null)
                {
                    AppSession.Token = token;
                    AppSession.CurrentUser = usuario;
                    return new Window(new AppShell());
                }
            }

            // No valid session — show login
            AppSession.Clear();
            SecureStorage.Remove("jwt_token");
            return new Window(new Views.LoginPage());
        }
    }
}
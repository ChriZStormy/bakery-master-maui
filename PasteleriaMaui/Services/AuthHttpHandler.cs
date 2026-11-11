using System.Net;
using System.Net.Http.Headers;

namespace PasteleriaMaui.Services
{
    public class AuthHttpHandler : DelegatingHandler
    {
        public AuthHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Automatically inject Bearer token on every outgoing request
            if (!string.IsNullOrEmpty(AppSession.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AppSession.Token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                AppSession.HasAuthError = true;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Sesión Expirada",
                        "Tu sesión ha caducado. Por favor, inicia sesión nuevamente.",
                        "OK");
                    AppSession.Clear();
                    SecureStorage.Remove("jwt_token");
                    Application.Current.MainPage = new Views.LoginPage();
                });
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                AppSession.HasAuthError = true;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Acceso Denegado",
                        "No tienes permisos de administrador para realizar esta acción.",
                        "OK");
                });
            }

            return response;
        }
    }
}

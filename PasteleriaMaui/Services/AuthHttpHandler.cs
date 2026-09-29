using System.Net;
using Microsoft.Maui.Controls;

namespace PasteleriaMaui.Services
{
    public class AuthHttpHandler : DelegatingHandler
    {
        public AuthHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                AppSession.HasAuthError = true;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Sesión Expirada", "Tu sesión ha caducado. Por favor, inicia sesión nuevamente.", "OK");
                    AppSession.CurrentUser = null;
                    AppSession.Token = null;
                    SecureStorage.Remove("jwt_token");
                    Application.Current.MainPage = new Views.LoginPage();
                });
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                AppSession.HasAuthError = true;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Acceso Denegado", "No tienes permisos de administrador para realizar esta acción.", "OK");
                });
            }

            return response;
        }
    }
}

using Microsoft.Extensions.Logging;

namespace PasteleriaMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static HttpClientHandler GetHttpClientHandler()
        {
            var handler = new HttpClientHandler();

#if DEBUG
            // Permitir conexiones HTTP sin certificado en desarrollo
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif

            return handler;
        }
    }
}

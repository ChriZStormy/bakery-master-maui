using Microsoft.Extensions.Logging;
using PasteleriaMaui.Services;
using PasteleriaMaui.ViewModels;
using PasteleriaMaui.Views;

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

            // --- Servicios ---
            builder.Services.AddSingleton<IAdminService, AdminService>();
            builder.Services.AddSingleton<AdminService>();
            builder.Services.AddSingleton<PastelApiService>();
            builder.Services.AddSingleton<TransaccionesService>();
            builder.Services.AddSingleton<AuthService>();

            // --- ViewModels ---
            builder.Services.AddTransient<GestionUsuariosViewModel>();
            builder.Services.AddTransient<PedidoPersonalizadoViewModel>();
            builder.Services.AddTransient<RegistrarPedidoViewModel>();
            builder.Services.AddTransient<ComprobantePedidoViewModel>();
            builder.Services.AddTransient<HistorialPedidosViewModel>();
            builder.Services.AddTransient<ComandaViewModel>();

            // --- Vistas ---
            builder.Services.AddTransient<GestionUsuariosPage>();
            builder.Services.AddTransient<PedidoPersonalizadoPage>();
            builder.Services.AddTransient<RegistrarPedidoPage>();
            builder.Services.AddTransient<ComprobantePedidoPage>();
            builder.Services.AddTransient<HistorialPedidosPage>();
            builder.Services.AddTransient<ComandaPage>();

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

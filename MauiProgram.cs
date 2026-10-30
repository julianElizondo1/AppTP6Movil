
using Microsoft.Extensions.Logging;
using AppTP6Movil.Services;

namespace AppTP6Movil
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddSingleton<CarritoService>();
            builder.Services.AddSingleton<RegistroService>();

            // Cliente HTTP para comunicarnos con la API
            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://progmovilwebapi.runasp.net/")
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

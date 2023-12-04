using BluetoothScanner.ViewModels;
using Microsoft.Extensions.Logging;
#if WINDOWS
using BluetoothScanner.Platforms.Windows;
#endif
namespace BluetoothScanner
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

#if WINDOWS
            builder.Services.AddSingleton<IBluetoothScanner, WindowsBluetoothScanner>();
#endif
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DeviceScannerViewModel>();
            return builder.Build();
        }
    }
}

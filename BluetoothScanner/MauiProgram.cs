using BluetoothScanner.ViewModels;
using Microsoft.Extensions.Logging;

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
            builder.Services.AddSingleton<IBluetoothScanner, BluetoothScanner.Platforms.Windows.WindowsBluetoothScanner>();
            builder.Services.AddSingleton<IBluetoothPermissionChecker, BluetoothScanner.Platforms.Windows.BluetoothPermissionChecker>();
#endif
#if ANDROID
            builder.Services.AddSingleton<IBluetoothScanner, BluetoothScanner.Platforms.Android.AndroidBluetoothScanner>();
            builder.Services.AddSingleton<IBluetoothPermissionChecker, BluetoothScanner.Platforms.Android.BluetoothPermissionChecker>();
#endif


            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DeviceScannerViewModel>();
            return builder.Build();
        }
    }
}

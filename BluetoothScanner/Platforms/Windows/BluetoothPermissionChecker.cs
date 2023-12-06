
namespace BluetoothScanner.Platforms.Windows
{
    public class BluetoothPermissionChecker : IBluetoothPermissionChecker
    {
        public async Task<bool> IsPermissionGranted()
        {
            await Task.CompletedTask;
            return true;
        }

        public async Task<PermissionStatus> RequestPermissionAsync()
        {
            await Task.CompletedTask;
            return PermissionStatus.Granted;
        }
    }
}

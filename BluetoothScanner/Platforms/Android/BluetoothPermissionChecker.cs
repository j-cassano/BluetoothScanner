namespace BluetoothScanner.Platforms.Android
{
    public class BluetoothPermissionChecker : IBluetoothPermissionChecker
    {
        public async Task<bool> IsPermissionGranted()
        {
            var status = await Permissions.CheckStatusAsync<BluetoothScanPermission>();

            return status == PermissionStatus.Granted;
        }

        public async Task<PermissionStatus> RequestPermissionAsync()
        {
            var status = await Permissions.RequestAsync<BluetoothScanPermission>();

            if (status == PermissionStatus.Granted)
                return status;

            await Shell.Current.DisplayAlert("Bluetooth permissions required.", "To use this app and scan for bluetooth devices you must go to your settings and allow bluetooth", "OK");

            return status;
        }
    }
}

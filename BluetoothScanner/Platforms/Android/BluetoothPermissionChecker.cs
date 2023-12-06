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
            return await Permissions.RequestAsync<BluetoothScanPermission>();
        }
    }
}

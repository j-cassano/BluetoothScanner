namespace BluetoothScanner
{
    public interface IBluetoothPermissionChecker
    {
      Task<PermissionStatus> RequestPermissionAsync();

      Task<bool> IsPermissionGranted();
    }
}

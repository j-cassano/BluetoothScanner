
namespace BluetoothScanner.Platforms.Windows
{
    public class WindowsBluetoothScanner : IBluetoothScanner
    {
        public event EventHandler? DeviceFound;

        public Task ScanForDevices()
        {
            throw new NotImplementedException();
        }

        public Task StopDeviceScan()
        {
            throw new NotImplementedException();
        }
    }
}

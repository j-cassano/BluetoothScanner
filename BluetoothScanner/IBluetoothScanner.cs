namespace BluetoothScanner
{
    public interface IBluetoothScanner
    {
        Task ScanForDevices();

        Task StopDeviceScan();

        event EventHandler<DeviceDiscoveredEventArgs> OnDeviceDiscovered;

        bool IsBluetoothOn();
    }
}

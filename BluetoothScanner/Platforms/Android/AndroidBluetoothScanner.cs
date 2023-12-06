using Android.Bluetooth;
using Android.Content;

namespace BluetoothScanner.Platforms.Android
{
    public class AndroidBluetoothScanner : IBluetoothScanner
    {
        public event EventHandler<DeviceDiscoveredEventArgs> OnDeviceDiscovered;
        private BluetoothManager? bluetoothManager;
        private BluetoothAdapter? bluetoothAdapter;
        private DeviceFoundReceiver? deviceFoundReceiver;

        public AndroidBluetoothScanner()
        {
            bluetoothManager = Platform.AppContext?.GetSystemService(Context.BluetoothService) as BluetoothManager;
            bluetoothAdapter = bluetoothManager?.Adapter;
        }

        public bool IsBluetoothOn()
        {   
            return bluetoothAdapter == null ? false : bluetoothAdapter.IsEnabled;
        }

        public async Task ScanForDevices()
        {
            deviceFoundReceiver = new DeviceFoundReceiver();
            deviceFoundReceiver.OnDeviceDiscovered += OnDeviceFound;
            Platform.AppContext?.RegisterReceiver(deviceFoundReceiver, new IntentFilter(BluetoothDevice.ActionFound));
            var bluetoothManager = Platform.AppContext?.GetSystemService(Context.BluetoothService) as BluetoothManager;
            var bluetoothAdapter = bluetoothManager?.Adapter;

            bluetoothAdapter?.StartDiscovery();
        }

        public async Task StopDeviceScan()
        {
            if (deviceFoundReceiver != null) deviceFoundReceiver.OnDeviceDiscovered -= OnDeviceFound;
            
            Platform.AppContext?.UnregisterReceiver(deviceFoundReceiver);
            bluetoothAdapter?.CancelDiscovery();
        }

        private void OnDeviceFound(object? sender, DeviceDiscoveredEventArgs eventArgs)
        {
            OnDeviceDiscovered?.Invoke(this, eventArgs);
        }
    }
}

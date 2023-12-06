using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothScanner.Platforms.Windows
{
    public class BluetoothScanner : IBluetoothScanner
    {
        public event EventHandler<DeviceDiscoveredEventArgs>? OnDeviceDiscovered;
        private readonly BluetoothLEAdvertisementWatcher watcher;

        public BluetoothScanner()
        {
            watcher = new BluetoothLEAdvertisementWatcher();
        }

        public async Task ScanForDevices()
        {
            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
        }

        public async Task StopDeviceScan()
        {
            watcher.Stop();
        }

        private async void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            DateTimeOffset timestamp = eventArgs.Timestamp;
            BluetoothLEAdvertisementType advertisementType = eventArgs.AdvertisementType;
          
            var deviceInfo = new DeviceInfo
            {
                Name = eventArgs.Advertisement.LocalName,
                RSSI = eventArgs.RawSignalStrengthInDBm,
                BluetoothAddress = eventArgs.BluetoothAddress.ToString()
            };

            var deviceDiscoveredEventArgs = new DeviceDiscoveredEventArgs
            {
                DeviceInfo = deviceInfo
            };

            OnDeviceDiscovered?.Invoke(this, deviceDiscoveredEventArgs);
        }

        public bool IsBluetoothOn()
        {
            return true;
        }
    }
}

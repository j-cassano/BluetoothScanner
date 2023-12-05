
using System.Diagnostics;
using Windows.Devices.Bluetooth.Advertisement;

namespace BluetoothScanner.Platforms.Windows
{
    public class WindowsBluetoothScanner : IBluetoothScanner
    {
        public event EventHandler<DeviceDiscoveredEventArgs>? OnDeviceDiscovered;
        private BluetoothLEAdvertisementWatcher watcher;

        public WindowsBluetoothScanner()
        {
            watcher = new BluetoothLEAdvertisementWatcher();
        }

        public async Task ScanForDevices()
        {
            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
            Debug.WriteLine("Max out of range timeout: " + watcher.MaxOutOfRangeTimeout);
            Debug.WriteLine("Min out of range timeout: " + watcher.MinOutOfRangeTimeout);
            Debug.WriteLine("Max sampling interval: " + watcher.MaxSamplingInterval);
            Debug.WriteLine("Min sampling interval: " + watcher.MinSamplingInterval);
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

    }
}

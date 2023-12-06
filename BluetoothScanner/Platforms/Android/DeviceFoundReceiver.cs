using Android.Bluetooth;
using Android.Content;

namespace BluetoothScanner.Platforms.Android
{
    public class DeviceFoundReceiver : BroadcastReceiver
    {
        public event EventHandler<DeviceDiscoveredEventArgs>? OnDeviceDiscovered;

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (BluetoothDevice.ActionFound == intent?.Action)
            {
                BluetoothDevice? device = intent?.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;
                var rssi = intent?.GetShortExtra(BluetoothDevice.ExtraRssi, short.MinValue);
                
                var deviceInfo = new DeviceInfo
                {
                    Name = device.Name,
                    RSSI = (int)rssi,
                    BluetoothAddress = device.Address
                };

                var deviceDiscoveredEventArgs = new DeviceDiscoveredEventArgs
                {
                    DeviceInfo = deviceInfo
                };


                OnDeviceDiscovered?.Invoke(this, deviceDiscoveredEventArgs);
            }
        }
    }
}

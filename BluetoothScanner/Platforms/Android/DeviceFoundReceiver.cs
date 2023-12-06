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
                BluetoothDevice? device = intent?.GetParcelableExtra(BluetoothDevice.ExtraDevice, Java.Lang.Class.FromType(typeof(BluetoothDevice))) as BluetoothDevice;
                var rssi = intent?.GetShortExtra(BluetoothDevice.ExtraRssi, short.MinValue);

                if (device != null)
                {
                    var deviceInfo = new DeviceInfo
                    {
                        Name = string.IsNullOrEmpty(device.Name) ? string.Empty : device.Name,
                        RSSI = rssi == null ? short.MinValue : (int)rssi,
                        BluetoothAddress = string.IsNullOrEmpty(device.Address) ? string.Empty : device.Address
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
}

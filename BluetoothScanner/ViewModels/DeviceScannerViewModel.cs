using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BluetoothScanner.ViewModels
{
    public class DeviceScannerViewModel : ObservableObject
    {
        private bool isScanning = false;
        private ObservableCollection<DeviceInfo> devices = new ObservableCollection<DeviceInfo>();
        private string scanButtonText = "Scan for devices";
        private IBluetoothScanner bluetoothScanner;
        private IBluetoothPermissionChecker bluetoothPermissionChecker;
        public IAsyncRelayCommand ScanDevicesCommand { get; }

        public DeviceScannerViewModel(IBluetoothScanner bluetoothScanner, IBluetoothPermissionChecker bluetoothPermissionChecker)
        {
            this.bluetoothScanner = bluetoothScanner;
            this.bluetoothPermissionChecker = bluetoothPermissionChecker;
            bluetoothScanner.OnDeviceDiscovered += OnDeviceFound;
            ScanDevicesCommand = new AsyncRelayCommand(ToggleBleScan);
        }

        public ObservableCollection<DeviceInfo> Devices
        {
            get => devices;
            set => SetProperty(ref devices, value);
        }

        public string ScanButtonText
        {
            get => scanButtonText;
            set => SetProperty(ref scanButtonText, value);
        }

        private void OnDeviceFound(Object sender, DeviceDiscoveredEventArgs eventArgs)
        {
            if (Devices.Any(x => x.BluetoothAddress == eventArgs.DeviceInfo.BluetoothAddress))
                return;

            eventArgs.DeviceInfo.Name = string.IsNullOrEmpty(eventArgs.DeviceInfo.Name) ? "Unknown device" : eventArgs.DeviceInfo.Name;
            MainThread.BeginInvokeOnMainThread(() => Devices.Add(eventArgs.DeviceInfo));
        }

        private async Task ToggleBleScan()
        {
            if (isScanning)
            {
                await StopScanning();
                return;
            }

            if (await bluetoothPermissionChecker.IsPermissionGranted() == false)
            {
                var status = await bluetoothPermissionChecker.RequestPermissionAsync();
                if (status == PermissionStatus.Granted)
                {
                    await StartScanning();
                }
            }

            await StartScanning();
        }

        private async Task StartScanning()
        {
            isScanning = true;
            await bluetoothScanner.ScanForDevices();
            ScanButtonText = "Stop Scanning";
        }

        private async Task StopScanning()
        {
            isScanning = false;
            await bluetoothScanner.StopDeviceScan();
            ScanButtonText = "Scan for devices";

        }
    }
}

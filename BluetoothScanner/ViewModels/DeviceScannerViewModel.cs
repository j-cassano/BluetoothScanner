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
        public IAsyncRelayCommand ScanDevicesCommand { get; }

        public DeviceScannerViewModel(IBluetoothScanner bluetoothScanner)
        {
            this.bluetoothScanner = bluetoothScanner;
            bluetoothScanner.OnDeviceDiscovered += OnDeviceFound;
            ScanDevicesCommand = new AsyncRelayCommand(StartScanning);
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

        private async Task StartScanning()
        {
            if (isScanning)
            {
                isScanning = false;
                await bluetoothScanner.StopDeviceScan();
                ScanButtonText = "Scan for devices";
                return;
            }

            isScanning = true;
            await bluetoothScanner.ScanForDevices();
            ScanButtonText = "Stop Scanning";
        }
    }
}

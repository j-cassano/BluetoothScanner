using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BluetoothScanner.ViewModels
{
    public class DeviceScannerViewModel : ObservableObject
    {
        private bool isScanning = false;
        private ObservableCollection<DeviceInfo> devices = [];
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

        ~DeviceScannerViewModel()
        {
            bluetoothScanner.OnDeviceDiscovered -= OnDeviceFound;
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

        private void OnDeviceFound(object? sender, DeviceDiscoveredEventArgs eventArgs)
        {
            try
            {
                if (Devices.Any(x => x.BluetoothAddress == eventArgs.DeviceInfo.BluetoothAddress))
                    return;

                eventArgs.DeviceInfo.Name = string.IsNullOrEmpty(eventArgs.DeviceInfo.Name) ? "Unknown device" : eventArgs.DeviceInfo.Name;
                MainThread.BeginInvokeOnMainThread(() => Devices.Add(eventArgs.DeviceInfo));
            }
            catch (Exception ex)
            {
                // In production this would use a logger that could upload the error message to a server for a developer to view
                // Logging to the user's phone only when running a debug build would not be ideal and this is just here to prevent an app crash
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private async Task ToggleBleScan()
        {
            try
            {
                if (isScanning)
                {
                    await StopScanning();
                    return;
                }

                if (await bluetoothPermissionChecker.IsPermissionGranted() == false)
                {
                    var status = await bluetoothPermissionChecker.RequestPermissionAsync();
                    if (status != PermissionStatus.Granted)
                        return;
                }

                Devices.Clear();
                await StartScanning();
            }
            catch (Exception ex)
            {
                // In production this would use a logger that could upload the error message to a server for a developer to view
                // Logging to the user's phone would not help and this is just here to prevent an app crash
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
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

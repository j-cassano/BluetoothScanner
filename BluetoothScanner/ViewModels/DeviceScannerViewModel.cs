using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BluetoothScanner.ViewModels
{
    public class DeviceScannerViewModel : ObservableObject
    {
        private bool isScanning = false;
        private ObservableCollection<string> items = new ObservableCollection<string>();
        private string scanButtonText = "Scan for devices";
        private IBluetoothScanner bluetoothScanner;
        public IAsyncRelayCommand ScanDevicesCommand { get; }

        public DeviceScannerViewModel(IBluetoothScanner bluetoothScanner)
        {
            this.bluetoothScanner = bluetoothScanner;
            Items.Add("test");
            Items.Add("test");
            Items.Add("test");

            ScanDevicesCommand = new AsyncRelayCommand(OnBluetoothScanClicked);
        }

        public ObservableCollection<string> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public string ScanButtonText
        {
            get => scanButtonText;
            set => SetProperty(ref scanButtonText, value);
        }

        private async Task OnBluetoothScanClicked()
        {
            if (isScanning)
            {
                isScanning = false;
                await bluetoothScanner.ScanForDevices();
                ScanButtonText = "Scan for devices";
                return;
            }

            isScanning = true;
            // todo start 

            ScanButtonText = "Stop Scanning";
        }
    }
}

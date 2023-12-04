namespace BluetoothScanner
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ViewModels.DeviceScannerViewModel deviceScannerViewModel)
        {
            InitializeComponent();
            BindingContext = deviceScannerViewModel;
        }
    }

}

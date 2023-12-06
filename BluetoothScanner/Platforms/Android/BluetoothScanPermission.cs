using static Microsoft.Maui.ApplicationModel.Permissions;

namespace BluetoothScanner.Platforms.Android
{
    public class BluetoothScanPermission : BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
                ("android.permission.BLUETOOTH_SCAN", true),
                ("android.permission.BLUETOOTH_CONNECT", true)

            }.ToArray();
    }
}

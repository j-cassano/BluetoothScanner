using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothScanner
{
    public class DeviceInfo
    {
        public int RSSI { get; set; }
        public string Name { get; set; }

        public string BluetoothAddress { get; set; }
    }
}

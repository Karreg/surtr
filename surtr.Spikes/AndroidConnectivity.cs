using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AndroidCtrl;
using AndroidCtrl.ADB;
using AndroidCtrl.Tools;

namespace surtr.Spikes
{
    public class AndroidConnectivity
    {
        public void Connect()
        {
            Deploy.ADB();
        }

        private void ShowDevices(IEnumerable<DataModelDevicesItem> devices)
        {
            var str = new StringBuilder();
            foreach (var device in devices)
            {
                str.Append(string.Format("{0}/{1}/{2}\n", device.Model, device.Product, device.Serial));
            }
            MessageBox.Show(str.ToString());
        }

        public void Start()
        {
            ADB.ConnectionMonitor.Callback += this.OnDeviceConnected;
            ADB.ConnectionMonitor.Start();
        }

        private void OnDeviceConnected(object sender, ConnectionMonitorArgs e)
        {
            this.ShowDevices(e.Devices);
        }
    }
}

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AndroidCtrl;
using AndroidCtrl.ADB;
using AndroidCtrl.Logging;
using AndroidCtrl.Tools;

namespace surtr.Spikes
{
    public class AndroidConnectivity
    {
        public void Connect()
        {
            Logger.Instance.Active = true;
            Logger.Instance.WriteParts = true;
            Deploy.ADB();
        }

        private void ShowDevices(IEnumerable<DataModelDevicesItem> devices)
        {
            var str = new StringBuilder();
            foreach (var device in devices)
            {
                str.Append(string.Format("{0}/{1}/{2}\n", device.Model, device.Product, device.Serial));

                var directories = ADB.Instance().Device.Directories("/").GetDirectories();
                foreach (var directory in directories)
                {
                    str.Append(string.Format("{0}\n", directory.Name));
                }

            }
            MessageBox.Show(str.ToString());
        }

        public void Start()
        {
            ADB.Start();
            ADB.ConnectionMonitor.Callback += this.OnDeviceConnected;
            ADB.ConnectionMonitor.Start();
        }

        private void OnDeviceConnected(object sender, ConnectionMonitorArgs e)
        {
            this.ShowDevices(e.Devices);
        }
    }
}

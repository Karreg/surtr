using System;
using System.Collections.Generic;
using AndroidCtrl;
using AndroidCtrl.ADB;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class DeviceMonitor : IDisposable
    {
        private readonly Dictionary<string, Device> devices;

        public DeviceMonitor()
        {
            this.devices = new Dictionary<string, Device>();
            ADB.ConnectionMonitor.Callback += this.OnDevice;
            if (!ADB.ConnectionMonitor.IsStarted())
            {
                ADB.ConnectionMonitor.Start();
            }
        }

        private event Action<Device> internalDevice;

        public event Action<Device> Device
        {
            add
            {
                foreach (var device in this.devices.Values)
                {
                    value(device);
                }
                this.internalDevice += value;
            }

            remove { this.internalDevice -= value; }
        }

        private void OnDevice(object sender, ConnectionMonitorArgs e)
        {
            foreach (var device in e.Devices)
            {
                var ldevice = new Device();
                ldevice.Id = device.Device;
                ldevice.Ip = device.IP;
                ldevice.Mode = device.Mode;
                ldevice.Model = device.Model;
                ldevice.Product = device.Product;
                ldevice.Serial = device.Serial;
                ldevice.State = device.State.ToString();
                devices[ldevice.Id] = ldevice;

                if (this.internalDevice != null)
                {
                    this.internalDevice(ldevice);
                }

            }
        }

        public void Dispose()
        {
            if (ADB.ConnectionMonitor.IsStarted())
            {
                ADB.ConnectionMonitor.Stop();
            }
        }
    }
}

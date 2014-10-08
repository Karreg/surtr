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

        private event Action<Device> InternalDevice;

        public event Action<Device> Device
        {
            add
            {
                foreach (var device in this.devices.Values)
                {
                    value(device);
                }
                this.InternalDevice += value;
            }

            remove { this.InternalDevice -= value; }
        }

        private void OnDevice(object sender, ConnectionMonitorArgs e)
        {
            foreach (var device in e.Devices)
            {
                var ldevice = new Device
                {
                    Id = device.Device,
                    Ip = device.IP,
                    Mode = device.Mode,
                    Model = device.Model,
                    Product = device.Product,
                    Serial = device.Serial,
                    State = device.State.ToString()
                };
                devices[ldevice.Id] = ldevice;

                if (this.InternalDevice != null)
                {
                    this.InternalDevice(ldevice);
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

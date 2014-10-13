using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using AndroidCtrl;
using AndroidCtrl.ADB;
using AndroidCtrl.Logging;
using AndroidCtrl.Tools;
using Michonne.Interfaces;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class DeviceMonitor : IDeviceMonitor
    {
        private readonly Dictionary<string, Device> devices;
        private readonly IUnitOfExecution dispatcher;

        public DeviceMonitor(IUnitOfExecution dispatcher)
        {
            this.devices = new Dictionary<string, Device>();
            this.dispatcher = dispatcher;
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

        public void Start()
        {
            dispatcher.Dispatch(() =>
            {
                Logger.Active = true; //default is false
                Logger.WriteParts = true; //default is false
                try
                {
                    Deploy.ADB();
                }
                catch (Exception e)
                {
                }

                ADB.Start();

                ADB.ConnectionMonitor.Callback += this.OnDevice;

                if (!ADB.ConnectionMonitor.IsStarted())
                {
                    ADB.ConnectionMonitor.Start();
                }
            });
        }

        private void OnDevice(object sender, ConnectionMonitorArgs e)
        {
            foreach (var device in e.Devices)
            {
                var ldevice = new Device
                {
                    Name = device.Device,
                    Ip = device.IP,
                    Mode = device.Mode,
                    Model = device.Model,
                    Product = device.Product,
                    Serial = device.Serial,
                    State = device.State.ToString()
                };
                devices[ldevice.Serial] = ldevice;

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
            ADB.Stop();
        }
    }
}

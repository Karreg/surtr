using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;

namespace surtr.AndroidCtrlTestModule.ViewModels
{
    public class DeviceMonitorViewModel : BindableBase, IDisposable
    {
        private readonly DeviceMonitor deviceMonitor;
        
        public DeviceMonitorViewModel(DeviceMonitor deviceMonitor)
        {
            this.Devices = new ObservableCollection<Device>();
            this.deviceMonitor = deviceMonitor;
            this.deviceMonitor.Device += this.OnDevice;
        }

        public ObservableCollection<Device> Devices { get; private set; }

        public void Dispose()
        {
            this.deviceMonitor.Device -= this.OnDevice;
        }
        
        private void OnDevice(Device device)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingDevice = this.Devices.FirstOrDefault(d => d.Id == device.Id);
                if (existingDevice == default(Device))
                {
                    this.Devices.Add(device);
                    this.OnPropertyChanged("Devices");
                }
                else
                {
                    existingDevice.Ip = device.Ip;
                    existingDevice.Mode = device.Mode;
                    existingDevice.Model = device.Model;
                    existingDevice.Product = device.Product;
                    existingDevice.Serial = device.Serial;
                    existingDevice.State = device.State;
                }
            });
        }
    }
}

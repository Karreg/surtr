using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;

namespace surtr.AndroidCtrlTestModule.ViewModels
{
    public class DeviceMonitorViewModel : BindableBase, IDisposable
    {
        private readonly DeviceMonitor deviceMonitor;
        private readonly FileTreeService fileTreeService;
        private Device selectedDevice;
        
        public DeviceMonitorViewModel(DeviceMonitor deviceMonitor, FileTreeService fileTreeService)
        {
            this.Devices = new ObservableCollection<Device>();
            this.deviceMonitor = deviceMonitor;
            this.deviceMonitor.Device += this.OnDevice;
            this.fileTreeService = fileTreeService;
        }

        public ObservableCollection<Device> Devices { get; private set; }

        public Device SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                this.OnPropertyChanged("SelectedDevice");
                this.fileTreeService.SetDevice(this.SelectedDevice);
            }
        }

        public void Dispose()
        {
            this.deviceMonitor.Device -= this.OnDevice;
        }
        
        private void OnDevice(Device device)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var existingDevice = this.Devices.FirstOrDefault(d => d.Serial == device.Serial);
                if (existingDevice == default(Device))
                {
                    this.Devices.Add(device);
                    this.OnPropertyChanged("Devices");
                }
                else
                {
                    existingDevice.Name = device.Name;
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

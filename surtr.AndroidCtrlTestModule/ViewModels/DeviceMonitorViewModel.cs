using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;

namespace surtr.AndroidCtrlTestModule.ViewModels
{
    public class DeviceMonitorViewModel : BindableBase
    {
        private string myProperty = string.Empty;
        private DeviceMonitor deviceMonitor;

        public DeviceMonitorViewModel(DeviceMonitor deviceMonitor)
        {
            this.MyProperty = "Property Test";
            this.deviceMonitor = deviceMonitor;
            this.deviceMonitor.Device += this.OnDevice;
        }

        public string MyProperty
        {
            get { return myProperty; }
            set
            {
                this.myProperty = value;
                this.OnPropertyChanged("MyProperty");
            }
        }

        private void OnDevice(Device obj)
        {
            this.MyProperty = obj.Id;
        }
    }
}

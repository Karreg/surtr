using System;

namespace surtr.AndroidCtrlTestModule.Services
{
    public interface IDeviceMonitor : IDisposable
    {
        event Action<Device> Device;

        void Start();
    }
}
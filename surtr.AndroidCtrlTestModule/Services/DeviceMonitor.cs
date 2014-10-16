using System;
using System.Collections.Generic;
using AndroidCtrl;
using AndroidCtrl.ADB;
using AndroidCtrl.Logging;
using AndroidCtrl.Tools;
using Common.Logging;
using Michonne.Interfaces;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class DeviceMonitor : IDeviceMonitor
    {
        private readonly Dictionary<string, Device> devices;
        private readonly IUnitOfExecution dispatcher;
        private readonly ILog logger;

        public DeviceMonitor(IUnitOfExecution dispatcher, ILog logger)
        {
            this.devices = new Dictionary<string, Device>();
            this.dispatcher = dispatcher;
            this.logger = logger;
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
                this.SetupLogger();

                if (!ADB.IsStarted())
                {
                    this.logger.Info("Deploying ADB...");
                    Deploy.ADB();
                    this.logger.Info("Starting ADB server...");
                    ADB.Start();
                }
                else
                {
                    this.logger.Info("ADB server is already started, we do nothing.");
                }

                ADB.ConnectionMonitor.Callback += this.OnDevice;

                if (!ADB.ConnectionMonitor.IsStarted())
                {
                    this.logger.InfoFormat("Starting ConnectionMonitor...");
                    ADB.ConnectionMonitor.Start();
                }
            });
        }

        private void SetupLogger()
        {
            Logger.Instance.Active = true; //default is false
            Logger.Instance.CallbackDebug += this.AdbDebug;
            Logger.Instance.CallbackInfo += this.AdbInfo;
            Logger.Instance.CallbackOutput += this.AdbInfo;
            Logger.Instance.CallbackWarning += this.AdbWarning;
            Logger.Instance.CallbackError += this.AdbError;
            this.SetLogLevel();
        }

        private void SetLogLevel()
        {
            if (this.logger.IsDebugEnabled)
            {
                Logger.Instance.SetLogLevel(IDLog.DEBUG);
            } else if (this.logger.IsInfoEnabled)
            {
                Logger.Instance.SetLogLevel(IDLog.INFO);
            }
            else if (this.logger.IsWarnEnabled)
            {
                Logger.Instance.SetLogLevel(IDLog.WARNING);
            }
            else
            {
                Logger.Instance.SetLogLevel(IDLog.ERROR);
            }
        }

        private void AdbError(object sender, LoggerArgs e)
        {
            this.logger.Error(e.Log.Msg);
        }

        private void AdbWarning(object sender, LoggerArgs e)
        {
            this.logger.Warn(e.Log.Msg);
        }

        private void AdbInfo(object sender, LoggerArgs e)
        {
            this.logger.Info(e.Log.Msg);
        }

        private void AdbDebug(object sender, LoggerArgs e)
        {
            this.logger.Debug(e.Log.Msg);
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
                this.logger.InfoFormat("Stopping ConnectionMonitor...");
                ADB.ConnectionMonitor.Stop();
            }
            this.logger.InfoFormat("Stopping ADB server...");
            ADB.Stop();
        }
    }
}

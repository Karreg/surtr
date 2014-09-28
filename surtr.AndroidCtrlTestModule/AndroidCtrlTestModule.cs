using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using surtr.AndroidCtrlTestModule.Services;
using surtr.Shared;

namespace surtr.AndroidCtrlTestModule
{
    public class AndroidCtrlTestModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;

        public AndroidCtrlTestModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
        }

        private void Dispose()
        {
            this.unityContainer.Resolve<DeviceMonitor>().Dispose();
        }

        public void Initialize()
        {
            this.RegisterServices();

            this.regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.DeviceMonitorView));
        }

        private void RegisterServices()
        {
            this.unityContainer.RegisterInstance(new DeviceMonitor(), this.ContainerControlledLifetimeManager);
        }

        public LifetimeManager ContainerControlledLifetimeManager { get { return new ContainerControlledLifetimeManager(); } }

        
    }
}

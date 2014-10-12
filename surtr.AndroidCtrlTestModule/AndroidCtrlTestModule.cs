using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Michonne.Implementation;
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
            this.unityContainer.Resolve<FileTreeService>().Dispose();
        }

        public void Initialize()
        {
            this.RegisterServices();

            this.regionManager.RegisterViewWithRegion("DeviceMonitorRegion", typeof(Views.DeviceMonitorView));
            this.regionManager.RegisterViewWithRegion("DeviceConfigurationRegion", typeof(Views.DeviceConfigurationView));
        }

        private void RegisterServices()
        {
            this.unityContainer.RegisterInstance(new UnitOfExecutionsFactory(), this.ContainerControlledLifetimeManager);

            var rootDispatcher = this.unityContainer.Resolve<UnitOfExecutionsFactory>().GetDedicatedThread();
            this.unityContainer.RegisterInstance(rootDispatcher);

            var deviceMonitor = new DeviceMonitor(rootDispatcher);
            deviceMonitor.Start();
            this.unityContainer.RegisterInstance(deviceMonitor, this.ContainerControlledLifetimeManager);

            var fileTreeService = new FileTreeService(rootDispatcher);
            this.unityContainer.RegisterInstance(fileTreeService, this.ContainerControlledLifetimeManager);
        }

        public LifetimeManager ContainerControlledLifetimeManager { get { return new ContainerControlledLifetimeManager(); } }

        
    }
}

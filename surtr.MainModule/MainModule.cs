using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace surtr.MainModule
{
    using Common.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    public class MainModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;

        public MainModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
        }

        public LifetimeManager ContainerControlledLifetimeManager { get { return new ContainerControlledLifetimeManager(); } }

        private void Dispose()
        {
        }

        public void Initialize()
        {
            this.RegisterServices();

            this.regionManager.RegisterViewWithRegion("LibraryRegion", typeof(Views.LibraryView));
            //this.regionManager.RegisterViewWithRegion("DeviceConfigurationRegion", typeof(Views.DeviceConfigurationView));
            
        }

        private void RegisterServices()
        {
            ILog logger = LogManager.GetLogger("surtr");
            this.unityContainer.RegisterInstance<ILog>(logger);

            logger.InfoFormat("Starting surtr...");

            //this.unityContainer.RegisterInstance(new UnitOfExecutionsFactory(), this.ContainerControlledLifetimeManager);

            //var rootDispatcher = this.unityContainer.Resolve<UnitOfExecutionsFactory>().GetDedicatedThread();
            //this.unityContainer.RegisterInstance(rootDispatcher);

            //this.unityContainer.RegisterType<IDeviceMonitor, DeviceMonitor>(this.ContainerControlledLifetimeManager);
            //this.unityContainer.Resolve<IDeviceMonitor>().Start();

            //this.unityContainer.RegisterType<IFileTreeService, FileTreeService>(this.ContainerControlledLifetimeManager);
        }
    }
}

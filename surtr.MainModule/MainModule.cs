namespace Surtr.MainModule
{
    using Common.Logging;
    using LibraryManagement.Implementation;
    using LibraryManagement.Interface;
    using Michonne.Implementation;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;

    using Surtr.MainModule.Services;

    /// <summary>
    /// The main module entry point
    /// </summary>
    public class MainModule : IModule
    {
        /// <summary>
        /// The region manager
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The unity container
        /// </summary>
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainModule"/> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="unityContainer">The unity container.</param>
        public MainModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
        }

        /// <summary>
        /// Gets the container controlled lifetime manager.
        /// </summary>
        /// <value>
        /// The container controlled lifetime manager.
        /// </value>
        public LifetimeManager ContainerControlledLifetimeManager
        {
            get { return new ContainerControlledLifetimeManager(); }
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            this.RegisterServices();

            this.regionManager.RegisterViewWithRegion("LibraryRegion", typeof(Views.LibraryView));
        }

        /// <summary>
        /// Registers the services.
        /// </summary>
        private void RegisterServices()
        {
            ILog logger = LogManager.GetLogger("surtr");
            this.unityContainer.RegisterInstance(logger);

            logger.InfoFormat("Starting surtr...");
            this.unityContainer.RegisterType<ISettingsService, SettingsService>(this.ContainerControlledLifetimeManager);
            this.unityContainer.RegisterType<IStoreService, StoreService>(this.ContainerControlledLifetimeManager);
            this.unityContainer.RegisterType<IScanService, ScanService>(this.ContainerControlledLifetimeManager);
            this.unityContainer.RegisterType<ISynchronizeService, SynchronizeService>(this.ContainerControlledLifetimeManager);

            this.unityContainer.RegisterInstance(new UnitOfExecutionsFactory(), this.ContainerControlledLifetimeManager);

            var rootDispatcher = this.unityContainer.Resolve<UnitOfExecutionsFactory>().GetDedicatedThread();
            this.unityContainer.RegisterInstance(rootDispatcher);
        }
    }
}

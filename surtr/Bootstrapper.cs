using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;

namespace surtr
{
    class Bootstrapper : UnityBootstrapper, IDisposable
    {
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainModule.MainModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Ensure we properly dispose of objects in the container at application exit
            Application.Current.Exit += (sender, e) => this.Container.Dispose();
        }

        public void Dispose()
        {
        }  
    }
}

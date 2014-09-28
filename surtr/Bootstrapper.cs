using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.UnityExtensions;
using surtr.Shared;

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
            moduleCatalog.AddModule(typeof(AndroidCtrlTestModule.AndroidCtrlTestModule));
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

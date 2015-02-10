namespace surtr.MainModule.ViewModels
{
    using System.IO;
    using System.Windows.Input;
    using LibraryManagement.Interface;
    using Microsoft.Practices.Prism.Commands;

    public class LibraryViewModel
    {
        private readonly IScanService scanService;
        private readonly IStoreService storeService;
        private readonly ISynchronizeService synchronizeService;

        public LibraryViewModel(IScanService scanService, IStoreService storeService, ISynchronizeService synchronizeService)
        {
            this.scanService = scanService;
            this.storeService = storeService;
            this.synchronizeService = synchronizeService;
            this.LibraryFolder = @"C:\Users\kryst_000\Documents\libraryTest";
            this.LoadCommand = new DelegateCommand(this.Load);

            this.RemoteLibraryFolder = @"C:\Users\kryst_000\Documents\libraryTestOutput";
            this.SynchronizeCommand = new DelegateCommand(this.Synchronize);
        }

        public string LibraryFolder { get; set; }

        public ILibrary Library { get; set; }

        public ICommand LoadCommand { get; private set; }

        public ICommand SynchronizeCommand { get; private set; }

        public string RemoteLibraryFolder { get; set; }

        public ILibrary RemoteLibrary { get; set; }

        private void Load()
        {
            var libraryFile = Path.Combine(this.LibraryFolder, "library.sur");
            if (File.Exists(libraryFile))
            {
                this.Library = this.storeService.Load(libraryFile);
            }
            else
            {
                this.Library = this.scanService.ScanLibrary(this.LibraryFolder);
                this.storeService.Store(this.Library, libraryFile);
            }
        }

        private void Synchronize()
        {
            var remoteLibraryFile = Path.Combine(this.RemoteLibraryFolder, "library.sur");
            if (File.Exists(remoteLibraryFile))
            {
                this.RemoteLibrary = this.storeService.Load(remoteLibraryFile);
            }
            else
            {
                this.RemoteLibrary = this.scanService.ScanLibrary(this.RemoteLibraryFolder);
                this.storeService.Store(this.RemoteLibrary, remoteLibraryFile);
            }

            var options = new SynchronizationOptions();
            options.LocalFileIsDeleted = SyncAction.DeleteFromRemote;
            options.LocalFileIsUpdated = SyncAction.CopyToRemote;
            options.RemoteFileIsDeleted = SyncAction.RemoveFromLocalLibrary;
            options.RemoteFileIsUpdated = SyncAction.Nothing;
            this.synchronizeService.Synchronize(this.Library, this.RemoteLibrary, options);
        }
    }
}

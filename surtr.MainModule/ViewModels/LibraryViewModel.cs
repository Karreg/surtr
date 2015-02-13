namespace surtr.MainModule.ViewModels
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using LibraryManagement.Annotations;
    using LibraryManagement.Implementation;
    using LibraryManagement.Interface;
    using Michonne.Interfaces;
    using Microsoft.Practices.Prism.Commands;

    public class LibraryViewModel : INotifyPropertyChanged
    {
        private readonly IScanService scanService;
        private readonly IStoreService storeService;
        private readonly ISynchronizeService synchronizeService;
        private string libraryFile;

        private string remoteLibraryFile;
        private readonly IUnitOfExecution rootDispatcher;
        private string status = string.Empty;
        private double currentSize;
        private string libraryFolder;
        private string remoteLibraryFolder;
        private string maxSize;

        public LibraryViewModel(IScanService scanService, IStoreService storeService,
            ISynchronizeService synchronizeService, IUnitOfExecution rootDispatcher)
        {
            this.scanService = scanService;
            this.storeService = storeService;
            this.synchronizeService = synchronizeService;
            this.synchronizeService.NewSyncItem += this.OnNewSyncItem;
            this.rootDispatcher = rootDispatcher;
            this.LibraryFolder = @"C:\Users\kryst_000\Documents\libraryTest";
            this.LoadCommand = new DelegateCommand(this.Load);
            this.SetFavoriteCommand = new DelegateCommand(this.SetFavorite);

            this.RemoteLibraryFolder = @"C:\Users\kryst_000\Documents\libraryTestOutput";
            this.SynchronizeCommand = new DelegateCommand(this.Synchronize);

            this.LibraryItems = new ObservableCollection<ILibraryItem>();
            this.SelectedLibraryItems = new ObservableCollection<ILibraryItem>();
            this.SyncItems = new ObservableCollection<ISyncItem>();
            this.ExecuteCommand = new DelegateCommand(this.Execute);

            this.MaxSize = "40";
        }

        public string LibraryFolder {
            get { return this.libraryFolder; }
            set
            {
                this.libraryFolder = value;
                this.OnPropertyChanged("LibraryFolder");
            }
        }

        public ILibrary Library { get; set; }

        public ObservableCollection<ILibraryItem> LibraryItems { get; private set; }

        public ObservableCollection<ILibraryItem> SelectedLibraryItems { get; set; }

        public ICommand LoadCommand { get; private set; }

        public ICommand SetFavoriteCommand { get; private set; }

        public ICommand SynchronizeCommand { get; private set; }

        public ICommand ExecuteCommand { get; private set; }

        public string RemoteLibraryFolder
        {
            get { return this.remoteLibraryFolder; }
            set
            {
                this.remoteLibraryFolder = value;
                this.OnPropertyChanged("RemoteLibraryFolder");
            }
        }

        public ILibrary RemoteLibrary { get; set; }

        public string Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        private void SetFavorite()
        {
            if (this.SelectedLibraryItems != null)
            {
                foreach (var item in this.SelectedLibraryItems)
                {
                    item.Favorite = !item.Favorite;
                }
            }
        }

        private void Load()
        {
            this.LibraryItems.Clear();

            this.libraryFile = Path.Combine(this.LibraryFolder, "library.sur");
            if (File.Exists(this.libraryFile))
            {
                this.Library = this.storeService.Load(this.libraryFile);
            }
            else
            {
                this.Library = this.scanService.ScanLibrary(this.LibraryFolder);
                this.storeService.Store(this.Library, this.libraryFile);
            }

            foreach (var libraryItem in this.Library.Items)
            {
                libraryItem.PropertyChanged += this.OnLibraryItemSelected;
                if (libraryItem.Favorite)
                {
                    this.CurrentSize += libraryItem.Size;
                }
                this.LibraryItems.Add(libraryItem);
            }
        }

        private void OnLibraryItemSelected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Favorite")
            {
                var item = (ILibraryItem) sender;
                if (item.Favorite)
                {
                    this.CurrentSize += item.Size;
                }
                else
                {
                    this.CurrentSize -= item.Size;
                }
            }
        }

        public double CurrentSize {
            get { return this.currentSize; }
            set
            {
                this.currentSize = value;
                this.OnPropertyChanged("CurrentSize");
                this.OnPropertyChanged("DisplayCurrentSize");
            }
        }

        public string DisplayCurrentSize
        {
            get { return string.Format("{0:0.00}", this.CurrentSize/1024); }
        }

        public string MaxSize
        {
            get { return this.maxSize; }
            set
            {
                this.maxSize = value;
                this.OnPropertyChanged("MaxSize");
            }
        }

        private void Synchronize()
        {
            this.SyncItems.Clear();
            this.storeService.Store(this.Library, this.libraryFile);

            this.remoteLibraryFile = Path.Combine(this.RemoteLibraryFolder, "library.sur");
            if (File.Exists(this.remoteLibraryFile))
            {
                this.RemoteLibrary = this.storeService.Load(this.remoteLibraryFile);
            }
            else
            {
                this.RemoteLibrary = this.scanService.ScanLibrary(this.RemoteLibraryFolder);
                this.storeService.Store(this.RemoteLibrary, this.remoteLibraryFile);
            }

            var options = new SynchronizationOptions();
            options.LocalFileIsDeleted = SyncAction.DeleteFromRemote;
            options.LocalFileIsUpdated = SyncAction.CopyToRemote;
            options.RemoteFileIsDeleted = SyncAction.RemoveFromLocalLibrary;
            options.RemoteFileIsUpdated = SyncAction.Nothing;
            this.synchronizeService.Synchronize(this.Library, this.RemoteLibrary, options);
        }

        private void OnNewSyncItem(ISyncItem syncItem)
        {
            this.SyncItems.Add(syncItem);
        }

        public ObservableCollection<ISyncItem> SyncItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        // TODO Move this in a service
        private void Execute()
        {
            double count = SyncItems.Count;
            double it = 0;
            this.rootDispatcher.Dispatch(() =>
            {
                foreach (var syncItem in SyncItems)
                {
                    switch (syncItem.Action)
                    {
                        case SyncAction.CopyToRemote:
                            var remoteItem = new LibraryItem(
                                this.RemoteLibraryFolder,
                                syncItem.Item.LibraryPath,
                                syncItem.Item.Filename);
                            this.RemoteLibrary.AddItem(remoteItem);
                            var src = syncItem.Item.FullPathFilename;
                            var dest = remoteItem.FullPathFilename;
                            if (!Directory.Exists(remoteItem.Path))
                            {
                                Directory.CreateDirectory(remoteItem.Path);
                            }

                            File.Copy(src, dest, true);
                            break;
                        case SyncAction.DeleteFromLocal:
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            this.Library.DeleteItem(syncItem.Name);
                            break;
                        case SyncAction.DeleteFromRemote:
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            break;
                        case SyncAction.RemoveFromLocalLibrary:
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            syncItem.Item.Favorite = false;
                            break;
                        case SyncAction.Nothing:
                        default:
                            break;
                    }

                    it++;
                    this.Status = string.Format("{0:P}", it / count);
                }
                
                this.storeService.Store(this.Library, this.libraryFile);
                this.storeService.Store(this.RemoteLibrary, this.remoteLibraryFile);
            });
        }
    }
}

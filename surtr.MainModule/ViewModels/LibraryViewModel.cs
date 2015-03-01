namespace surtr.MainModule.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Runtime.CompilerServices;
    using System.Security.AccessControl;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Common.Logging.Configuration;
    using LibraryManagement.Annotations;
    using LibraryManagement.Implementation;
    using LibraryManagement.Interface;
    using Michonne.Interfaces;
    using Microsoft.Practices.Prism;
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
        private string filter;
        private IList<ISyncItem> syncItems;
        private bool showNullAction;
        private string titleFilter;
        private readonly Dispatcher appDispatcher;

        public LibraryViewModel(IScanService scanService, IStoreService storeService,
            ISynchronizeService synchronizeService, IUnitOfExecution rootDispatcher)
        {
            this.appDispatcher = Application.Current.Dispatcher;

            this.scanService = scanService;
            this.scanService.CurrentDirectory += this.OnCurrentDirectory;
            this.storeService = storeService;
            this.storeService.Loaded += () => this.Status = "Loaded";
            this.storeService.Loading += () => this.Status = "Loading...";
            this.storeService.Saved += () => this.Status = "Saved";
            this.storeService.Saving += p => this.Status = string.Format("Saving... {0:P}", p);
            this.synchronizeService = synchronizeService;
            this.synchronizeService.NewSyncItem += this.OnNewSyncItem;
            this.rootDispatcher = rootDispatcher;

            var loadedFolder = ConfigurationManager.AppSettings["LocalLibraryFolder"];
            this.LibraryFolder = !string.IsNullOrEmpty(loadedFolder) ? loadedFolder : string.Empty;
            
            this.LoadCommand = new DelegateCommand(this.Load);
            this.SetFavoriteCommand = new DelegateCommand(this.SetFavorite);
            this.AddFavoriteCommand = new DelegateCommand(() => this.Favorite(true));
            this.RemoveFavoriteCommand = new DelegateCommand(() => this.Favorite(false));
            this.OpenCommand = new DelegateCommand(this.Open);
            this.DeleteCommand = new DelegateCommand(this.Delete);
            this.SaveCommand = new DelegateCommand(this.Save);

            loadedFolder = ConfigurationManager.AppSettings["RemoteLibraryFolder"];
            this.RemoteLibraryFolder = !string.IsNullOrEmpty(loadedFolder) ? loadedFolder : string.Empty;

            this.SynchronizeCommand = new DelegateCommand(this.Synchronize);

            this.LibraryItems = new ObservableCollection<ILibraryItem>();
            this.SelectedLibraryItems = new ObservableCollection<ILibraryItem>();

            this.syncItems = new List<ISyncItem>();
            this.SyncItems = new ObservableCollection<ISyncItem>();
            this.LibraryFilters = new ObservableCollection<string>();
            this.ShowNullAction = false;

            this.ExecuteCommand = new DelegateCommand(this.Execute);

            var loadedSize = ConfigurationManager.AppSettings["MaximumRemoteSize"];
            this.MaxSize = !string.IsNullOrEmpty(loadedSize) ? loadedSize : "10";
        }

        public bool ShowNullAction
        {
            get { return this.showNullAction; }
            set
            {
                if (this.showNullAction != value)
                {
                    this.showNullAction = value;
                    this.OnPropertyChanged("ShowNullAction");
                    this.FilterSyncItems();
                }
            }
        }

        public ObservableCollection<string> LibraryFilters { get; set; }

        public string SelectedFilter
        {
            get { return this.filter; }
            set
            {
                if (value != null && !value.Equals(this.filter))
                {
                    this.filter = value;
                    this.OnPropertyChanged("SelectedFilter");
                    this.FilterLibrary();
                }
            }
        }

        public string TitleFilter
        {
            get { return this.titleFilter; }
            set
            {
                if (value != null && !value.Equals(this.titleFilter))
                {
                    this.titleFilter = value;
                    this.OnPropertyChanged("TitleFilter");
                    this.FilterLibrary();
                }
            }
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

        public ILibraryItem SelectedLibraryItem { get; set; }

        public ICommand LoadCommand { get; private set; }

        public ICommand SetFavoriteCommand { get; private set; }

        public ICommand AddFavoriteCommand { get; private set; }

        public ICommand RemoveFavoriteCommand { get; private set; }

        public ICommand SynchronizeCommand { get; private set; }

        public ICommand ExecuteCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

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

        private void Favorite(bool favorite)
        {
            if (this.SelectedLibraryItems != null)
            {
                foreach (var item in this.SelectedLibraryItems)
                {
                    item.Favorite = favorite;
                }
            }
        }

        private void Save()
        {
            this.storeService.Store(this.Library, this.libraryFile);
        }

        private void Open()
        {
            if (this.SelectedLibraryItem != null)
            {
                var openCommand = string.Format("{0} {1}", @"CDisplayEx\CDisplayEx.exe", this.SelectedLibraryItem.FullPathFilename);
                var p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = @"CDisplayEx\CDisplayEx.exe";
                p.StartInfo.Arguments = string.Format("\"{0}\"", this.SelectedLibraryItem.FullPathFilename);
                p.Start();
            }
        }

        private void Delete()
        {
            var itemsToDelete = this.SelectedLibraryItems.ToList();
            foreach (var libraryItem in itemsToDelete)
            {
                this.LibraryItems.Remove(libraryItem);
                this.Library.DeleteItem(libraryItem.Name);
            }
        }

        private void Load()
        {
            this.LibraryItems.Clear();
            this.LibraryFilters.Clear();
            this.LibraryFilters.Add(string.Empty);
            this.CurrentSize = 0;

            this.rootDispatcher.Dispatch(()
                =>
                {
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

                        ILibraryItem item = libraryItem;
                        this.appDispatcher.BeginInvoke((Action)(() =>
                        {
                            this.LibraryItems.Add(item);
                            if (!this.LibraryFilters.Contains(item.LibraryPath))
                            {
                                this.LibraryFilters.Add(item.LibraryPath);
                            }
                        }));
                    }
                });
        }

        private void FilterLibrary()
        {
            if (this.Library != null)
            {
                this.LibraryItems.Clear();
                if (!string.IsNullOrEmpty(this.filter))
                {
                    foreach (var item in this.Library.Items.Where(i => i.LibraryPath.Equals(this.filter)))
                    {
                        if (this.TitleContainsFilter(item.Filename))
                        {
                            this.LibraryItems.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in this.Library.Items)
                    {
                        if (this.TitleContainsFilter(item.Filename))
                        {
                            this.LibraryItems.Add(item);
                        }
                    }
                }
            }
        }

        private bool TitleContainsFilter(string title)
        {
            if (!string.IsNullOrEmpty(this.titleFilter))
            {
                return title.Contains(this.titleFilter);
            }

            return true;
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

        private void FilterSyncItems()
        {
            this.SyncItems.Clear();
            if (this.showNullAction)
            {
                this.SyncItems.AddRange(syncItems);
            }
            else
            {
                this.SyncItems.AddRange(syncItems.Where(i => i.Action != SyncAction.Nothing));
            }
        }

        private void Synchronize()
        {
            this.SyncItems.Clear();
            this.syncItems.Clear();

            this.rootDispatcher.Dispatch(() =>
            {
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
            });
        }

        private void OnNewSyncItem(ISyncItem syncItem)
        {
            this.syncItems.Add(syncItem);
            if (this.showNullAction || syncItem.Action != SyncAction.Nothing)
            {
                this.appDispatcher.BeginInvoke((Action) (() => this.SyncItems.Add(syncItem)));
            }
        }

        private void OnCurrentDirectory(string directory)
        {
            this.Status = directory;
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
                    it++;
                    double it1 = it;

                    switch (syncItem.Action)
                    {
                        case SyncAction.CopyToRemote:

                            this.Status = string.Format("Copying {0} ({1:P})", syncItem.Item.LibraryPath, it1 / count);

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
                            this.Status = string.Format("Deleting local {0} ({1:P})", syncItem.Item.LibraryPath, it1 / count);
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            this.Library.DeleteItem(syncItem.Name);
                            break;
                        case SyncAction.DeleteFromRemote:
                            this.Status = string.Format("Deleting remote {0} ({1:P})", syncItem.Item.LibraryPath, it1 / count);
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            break;
                        case SyncAction.RemoveFromLocalLibrary:
                            this.Status = string.Format("Removing {0} ({1:P})", syncItem.Item.LibraryPath, it1 / count);
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            syncItem.Item.Favorite = false;
                            break;
                        case SyncAction.Nothing:
                        default:
                            break;
                    }
                }
                
                this.storeService.Store(this.Library, this.libraryFile);
                this.storeService.Store(this.RemoteLibrary, this.remoteLibraryFile);

                this.Status = "Sync'd.";
            });
        }
    }
}

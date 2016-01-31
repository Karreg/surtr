namespace Surtr.MainModule.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Michonne.Interfaces;

    using Prism.Commands;

    using Surtr.LibraryManagement.Annotations;
    using Surtr.LibraryManagement.Implementation;
    using Surtr.LibraryManagement.Interface;
    using Surtr.MainModule.Services;

    /// <summary>
    /// The library view model.
    /// </summary>
    public class LibraryViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The scan service.
        /// </summary>
        private readonly IScanService scanService;

        /// <summary>
        /// The store service.
        /// </summary>
        private readonly IStoreService storeService;

        /// <summary>
        /// The synchronize service.
        /// </summary>
        private readonly ISynchronizeService synchronizeService;

        /// <summary>
        /// The root dispatcher.
        /// </summary>
        private readonly IUnitOfExecution rootDispatcher;

        /// <summary>
        /// The app dispatcher.
        /// </summary>
        private readonly Dispatcher appDispatcher;

        /// <summary>
        /// The sync items.
        /// </summary>
        private readonly IList<ISyncItem> syncItems;

        /// <summary>
        /// The settings service
        /// </summary>
        private readonly ISettingsService settingsService;

        /// <summary>
        /// The library file.
        /// </summary>
        private string libraryFile;

        /// <summary>
        /// The remote library file.
        /// </summary>
        private string remoteLibraryFile;

        /// <summary>
        /// The status.
        /// </summary>
        private string status = string.Empty;

        /// <summary>
        /// The current size.
        /// </summary>
        private double currentSize;

        /// <summary>
        /// The library folder.
        /// </summary>
        private string libraryFolder;

        /// <summary>
        /// The remote library folder.
        /// </summary>
        private string remoteLibraryFolder;

        /// <summary>
        /// The max size.
        /// </summary>
        private string maxSize;

        /// <summary>
        /// The filter.
        /// </summary>
        private string filter;

        /// <summary>
        /// The show null action.
        /// </summary>
        private bool showNullAction;

        /// <summary>
        /// The title filter.
        /// </summary>
        private string titleFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryViewModel" /> class.
        /// </summary>
        /// <param name="scanService">The scan service.</param>
        /// <param name="storeService">The store service.</param>
        /// <param name="synchronizeService">The synchronize service.</param>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="rootDispatcher">The root dispatcher.</param>
        public LibraryViewModel(
            IScanService scanService,
            IStoreService storeService,
            ISynchronizeService synchronizeService,
            ISettingsService settingsService,
            IUnitOfExecution rootDispatcher)
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
            this.settingsService = settingsService;
            this.rootDispatcher = rootDispatcher;

            this.LibraryFolder = this.settingsService.LocalLibraryFolder;
            
            this.LoadCommand = new DelegateCommand(this.Load);
            this.SetFavoriteCommand = new DelegateCommand(this.SetFavorite);
            this.AddFavoriteCommand = new DelegateCommand(() => this.Favorite(true));
            this.RemoveFavoriteCommand = new DelegateCommand(() => this.Favorite(false));
            this.OpenCommand = new DelegateCommand(this.Open);
            this.DeleteCommand = new DelegateCommand(this.Delete);
            this.SaveCommand = new DelegateCommand(this.Save);

            this.RemoteLibraryFolder = this.settingsService.RemoteLibraryFolder;

            this.SynchronizeCommand = new DelegateCommand(this.Synchronize);

            this.LibraryItems = new ObservableCollection<ILibraryItem>();
            this.SelectedLibraryItems = new ObservableCollection<ILibraryItem>();

            this.syncItems = new List<ISyncItem>();
            this.SyncItems = new ObservableCollection<ISyncItem>();
            this.LibraryFilters = new ObservableCollection<string>();
            this.ShowNullAction = false;

            this.ExecuteCommand = new DelegateCommand(this.Execute);

            this.MaxSize = this.settingsService.MaximumRemoteSize;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether show null action.
        /// </summary>
        public bool ShowNullAction
        {
            get
            {
                return this.showNullAction;
            }

            set
            {
                if (this.showNullAction != value)
                {
                    this.showNullAction = value;
                    this.OnPropertyChanged();
                    this.FilterSyncItems();
                }
            }
        }

        /// <summary>
        /// Gets or sets the library filters.
        /// </summary>
        public ObservableCollection<string> LibraryFilters { get; set; }

        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        public string SelectedFilter
        {
            get
            {
                return this.filter;
            }

            set
            {
                if (value != null && !value.Equals(this.filter))
                {
                    this.filter = value;
                    this.OnPropertyChanged();
                    this.FilterLibrary();
                }
            }
        }

        /// <summary>
        /// Gets or sets the title filter.
        /// </summary>
        public string TitleFilter
        {
            get
            {
                return this.titleFilter;
            }

            set
            {
                if (value != null && !value.Equals(this.titleFilter))
                {
                    this.titleFilter = value;
                    this.OnPropertyChanged();
                    this.FilterLibrary();
                }
            }
        }

        /// <summary>
        /// Gets or sets the library folder.
        /// </summary>
        public string LibraryFolder
        {
            get
            {
                return this.libraryFolder;
            }

            set
            {
                this.libraryFolder = value;
                this.OnPropertyChanged();
                this.settingsService.LocalLibraryFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public ILibrary Library { get; set; }

        /// <summary>
        /// Gets the library items.
        /// </summary>
        public ObservableCollection<ILibraryItem> LibraryItems { get; private set; }

        /// <summary>
        /// Gets or sets the selected library items.
        /// </summary>
        public ObservableCollection<ILibraryItem> SelectedLibraryItems { get; set; }

        /// <summary>
        /// Gets or sets the selected library item.
        /// </summary>
        public ILibraryItem SelectedLibraryItem { get; set; }

        /// <summary>
        /// Gets the load command.
        /// </summary>
        public ICommand LoadCommand { get; private set; }

        /// <summary>
        /// Gets the set favorite command.
        /// </summary>
        public ICommand SetFavoriteCommand { get; private set; }

        /// <summary>
        /// Gets the add favorite command.
        /// </summary>
        public ICommand AddFavoriteCommand { get; private set; }

        /// <summary>
        /// Gets the remove favorite command.
        /// </summary>
        public ICommand RemoveFavoriteCommand { get; private set; }

        /// <summary>
        /// Gets the synchronize command.
        /// </summary>
        public ICommand SynchronizeCommand { get; private set; }

        /// <summary>
        /// Gets the execute command.
        /// </summary>
        public ICommand ExecuteCommand { get; private set; }

        /// <summary>
        /// Gets the save command.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets the open command.
        /// </summary>
        public ICommand OpenCommand { get; private set; }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets or sets the remote library folder.
        /// </summary>
        public string RemoteLibraryFolder
        {
            get
            {
                return this.remoteLibraryFolder;
            }

            set
            {
                this.remoteLibraryFolder = value;
                this.OnPropertyChanged();
                this.settingsService.RemoteLibraryFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the remote library.
        /// </summary>
        public ILibrary RemoteLibrary { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the current size.
        /// </summary>
        public double CurrentSize
        {
            get
            {
                return this.currentSize;
            }

            set
            {
                this.currentSize = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("DisplayCurrentSize");
            }
        }

        /// <summary>
        /// Gets the display current size.
        /// </summary>
        public string DisplayCurrentSize
        {
            get
            {
                return string.Format("{0:0.00}", this.CurrentSize / 1024);
            }
        }

        /// <summary>
        /// Gets or sets the max size.
        /// </summary>
        public string MaxSize
        {
            get
            {
                return this.maxSize;
            }

            set
            {
                this.maxSize = value;
                this.OnPropertyChanged();
                this.settingsService.MaximumRemoteSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the sync items.
        /// </summary>
        public ObservableCollection<ISyncItem> SyncItems { get; set; }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The set favorite.
        /// </summary>
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

        /// <summary>
        /// Set the selection to be favorite.
        /// </summary>
        /// <param name="favorite">
        /// The favorite.
        /// </param>
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

        /// <summary>
        /// The save.
        /// </summary>
        private void Save()
        {
            this.storeService.Store(this.Library, this.libraryFile);
        }

        /// <summary>
        /// The open.
        /// </summary>
        private void Open()
        {
            if (this.SelectedLibraryItem != null)
            {
                var p = new Process
                            {
                                StartInfo =
                                    {
                                        UseShellExecute = false,
                                        FileName = @"CDisplayEx\CDisplayEx.exe",
                                        Arguments =
                                            string.Format(
                                                "\"{0}\"",
                                                this.SelectedLibraryItem.FullPathFilename)
                                    }
                            };
                p.Start();
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        private void Delete()
        {
            var itemsToDelete = this.SelectedLibraryItems.ToList();
            foreach (var libraryItem in itemsToDelete)
            {
                this.LibraryItems.Remove(libraryItem);
                this.Library.DeleteItem(libraryItem.Name);
            }
        }

        /// <summary>
        /// The load.
        /// </summary>
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

        /// <summary>
        /// The filter library.
        /// </summary>
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

        /// <summary>
        /// The title contains filter.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool TitleContainsFilter(string title)
        {
            if (!string.IsNullOrEmpty(this.titleFilter))
            {
                return title.Contains(this.titleFilter);
            }

            return true;
        }

        /// <summary>
        /// The on library item selected.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnLibraryItemSelected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Favorite")
            {
                var item = (ILibraryItem)sender;
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

        /// <summary>
        /// The filter sync items.
        /// </summary>
        private void FilterSyncItems()
        {
            this.SyncItems.Clear();
            this.SyncItems.AddRange(
                this.showNullAction ? this.syncItems : this.syncItems.Where(i => i.Action != SyncAction.Nothing));
        }

        /// <summary>
        /// The synchronize.
        /// </summary>
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

                var options = new SynchronizationOptions
                                  {
                                      LocalFileIsDeleted = SyncAction.DeleteFromRemote,
                                      LocalFileIsUpdated = SyncAction.CopyToRemote,
                                      RemoteFileIsDeleted = SyncAction.RemoveFromLocalLibrary,
                                      RemoteFileIsUpdated = SyncAction.Nothing
                                  };
                this.Status = "Comparing...";
                this.synchronizeService.Synchronize(this.Library, this.RemoteLibrary, options);
                this.Status = "Done";
            });
        }

        /// <summary>
        /// The on new sync item.
        /// </summary>
        /// <param name="syncItem">
        /// The sync item.
        /// </param>
        private void OnNewSyncItem(ISyncItem syncItem)
        {
            this.syncItems.Add(syncItem);
            if (this.showNullAction || syncItem.Action != SyncAction.Nothing)
            {
                this.appDispatcher.BeginInvoke((Action)(() => this.SyncItems.Add(syncItem)));
            }
        }

        /// <summary>
        /// The on current directory.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        private void OnCurrentDirectory(string directory)
        {
            this.Status = directory;
        }

        /// <summary>
        /// The execute.
        /// TODO Move this in a service
        /// </summary>
        private void Execute()
        {
            double count = this.SyncItems.Count;
            double it = 0;
            this.rootDispatcher.Dispatch(() =>
            {
                foreach (var syncItem in this.SyncItems.ToList())
                {
                    it++;
                    double it1 = it;
                    var item = syncItem;

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
                            this.Status = string.Format("Deleting remote {0} ({1:P})", syncItem.RemoteItem.LibraryPath, it1 / count);
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            break;
                        case SyncAction.RemoveFromLocalLibrary:
                            this.Status = string.Format("Removing {0} ({1:P})", syncItem.Item.LibraryPath, it1 / count);
                            this.RemoteLibrary.DeleteItem(syncItem.Name);
                            syncItem.Item.Favorite = false;
                            break;
                    }

                    this.appDispatcher.BeginInvoke((Action)(() => this.SyncItems.Remove(item)));
                }
                
                this.storeService.Store(this.Library, this.libraryFile);
                this.storeService.Store(this.RemoteLibrary, this.remoteLibraryFile);

                this.Status = "Sync'd.";
            });
        }
    }
}

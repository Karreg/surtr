namespace Surtr.LibraryManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Interface;

    /// <summary>
    /// The synchronize service.
    /// </summary>
    public class SynchronizeService : ISynchronizeService
    {
        /// <summary>
        /// The store service.
        /// </summary>
        private readonly IStoreService storeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizeService"/> class.
        /// </summary>
        /// <param name="storeService">
        /// The store service.
        /// </param>
        public SynchronizeService(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        /// <summary>
        /// The new sync item.
        /// </summary>
        public event Action<ISyncItem> NewSyncItem;

        /// <summary>
        /// The synchronize.
        /// </summary>
        /// <param name="library">
        /// The library.
        /// </param>
        /// <param name="remoteFolder">
        /// The remote folder.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public void Synchronize(ILibrary library, string remoteFolder, SynchronizationOptions options)
        {
            var remoteFile = Path.Combine(remoteFolder, "sync.sur");
            if (File.Exists(remoteFile))
            {
                var remoteLibrary = this.storeService.Load(remoteFile);
                this.Synchronize(library, remoteLibrary, options);
            }
        }

        /// <summary>
        /// The synchronize.
        /// </summary>
        /// <param name="library">
        /// The library.
        /// </param>
        /// <param name="remoteLibrary">
        /// The remote library.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public void Synchronize(ILibrary library, ILibrary remoteLibrary, SynchronizationOptions options)
        {
            var syncItems = new Dictionary<string, ISyncItem>();
            foreach (var libraryItem in library.Items.Where(i => i.Favorite).ToList())
            {
                if (libraryItem.Exists)
                {
                    var syncItem = new SyncItem { Item = libraryItem, Name = libraryItem.Name };
                    syncItems.Add(libraryItem.Name, syncItem);
                }
                else
                {
                    library.RemoveItem(libraryItem.Name);
                }
            }

            foreach (var remoteItem in remoteLibrary.Items)
            {
                ISyncItem syncItem;
                if (!syncItems.TryGetValue(remoteItem.Name, out syncItem))
                {
                    syncItem = new SyncItem { Name = remoteItem.Name };
                    syncItems.Add(remoteItem.Name, syncItem);
                }

                syncItem.RemoteItem = remoteItem;
            }

            foreach (var syncItem in syncItems.Values)
            {
                this.SetSyncAction(syncItem, options);
                if (this.NewSyncItem != null)
                {
                    this.NewSyncItem(syncItem);
                }
            }
        }

        /// <summary>
        /// The set sync action.
        /// </summary>
        /// <param name="syncItem">
        /// The sync item.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        public void SetSyncAction(ISyncItem syncItem, SynchronizationOptions options)
        {
            if (syncItem.Item == null && syncItem.RemoteItem == null)
            {
                // Should not happen
            }

            if (syncItem.Item == null && syncItem.RemoteItem != null)
            {
                syncItem.Action = SyncAction.DeleteFromRemote;
            }

            if (syncItem.Item != null && syncItem.RemoteItem == null)
            {
                syncItem.Action = SyncAction.CopyToRemote;
            }

            if (syncItem.Item != null && syncItem.RemoteItem != null)
            {
                var item = syncItem.Item;
                var remoteItem = syncItem.RemoteItem;
                if (!item.Exists && !remoteItem.Exists)
                {
                    // Should not happen
                    syncItem.Action = SyncAction.DeleteFromLocal;
                }

                if (item.Exists && !remoteItem.Exists)
                {
                    syncItem.Action = options.RemoteFileIsDeleted;
                }

                if (!item.Exists && remoteItem.Exists)
                {
                    syncItem.Action = options.LocalFileIsDeleted;
                }

                if (item.Exists && remoteItem.Exists)
                {
                    if (item.LastModificationDate > remoteItem.LastModificationDate)
                    {
                        syncItem.Action = options.LocalFileIsUpdated;
                    }

                    if (item.LastModificationDate < remoteItem.LastModificationDate)
                    {
                        syncItem.Action = options.RemoteFileIsUpdated;
                    }
                }
            }
        }
    }
}
namespace surtr.LibraryManagement.Interface
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SynchronizeService : ISynchronizeService
    {
        private readonly IStoreService storeService;

        public SynchronizeService(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public event Action<ISyncItem> NewSyncItem;

        public void Synchronize(ILibrary library, string remoteFolder, SynchronizationOptions options)
        {
            var remoteFile = Path.Combine(remoteFolder, "sync.sur");
            if (File.Exists(remoteFile))
            {
                var remoteLibrary = storeService.Load(remoteFile);
                this.Synchronize(library, remoteLibrary, options);
            }
        }

        public void Synchronize(ILibrary library, ILibrary remoteLibrary, SynchronizationOptions options)
        {
            var syncItems = new Dictionary<string, ISyncItem>();
            foreach (var libraryItem in library.Items.ToList())
            {
                if (libraryItem.Exists)
                {
                    var syncItem = new SyncItem {Item = libraryItem};
                    syncItems.Add(libraryItem.Name, syncItem);
                }
                else
                {
                    library.RemoveItem(libraryItem);
                }
            }

            foreach (var remoteItem in remoteLibrary.Items)
            {
                ISyncItem syncItem;
                if (!syncItems.TryGetValue(remoteItem.Name, out syncItem))
                {
                    syncItem = new SyncItem();
                    syncItems.Add(remoteItem.Name, syncItem);
                }

                syncItem.RemoteItem = remoteItem;
            }

            foreach (var syncItem in syncItems.Values)
            {
                this.SetSyncAction(syncItem, options);
                this.NewSyncItem(syncItem);
            }
        }

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
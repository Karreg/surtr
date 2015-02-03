namespace surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Synchronization options
    /// </summary>
    public class SynchronizationOptions
    {
        public SyncAction RemoteFileIsDeleted { get; set; }

        public SyncAction LocalFileIsDeleted { get; set; }

        public SyncAction RemoteFileIsUpdated { get; set; }

        public SyncAction LocalFileIsUpdated { get; set; }
    }
}
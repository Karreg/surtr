namespace Surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Synchronization options
    /// </summary>
    public class SynchronizationOptions
    {
        /// <summary>
        /// Gets or sets the remote file is deleted default action.
        /// </summary>
        /// <value>
        /// The remote file is deleted.
        /// </value>
        public SyncAction RemoteFileIsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the local file is deleted default action.
        /// </summary>
        /// <value>
        /// The local file is deleted.
        /// </value>
        public SyncAction LocalFileIsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the remote file is updated default action.
        /// </summary>
        /// <value>
        /// The remote file is updated.
        /// </value>
        public SyncAction RemoteFileIsUpdated { get; set; }

        /// <summary>
        /// Gets or sets the local file is updated default action.
        /// </summary>
        /// <value>
        /// The local file is updated.
        /// </value>
        public SyncAction LocalFileIsUpdated { get; set; }
    }
}
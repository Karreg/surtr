namespace Surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Interface for sync item.
    /// </summary>
    public interface ISyncItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        SyncAction Action { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        ILibraryItem Item { get; set; }

        /// <summary>
        /// Gets or sets the remote item.
        /// </summary>
        /// <value>
        /// The remote item.
        /// </value>
        ILibraryItem RemoteItem { get; set; }
    }
}
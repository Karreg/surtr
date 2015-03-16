namespace Surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Implementation of SyncItem.
    /// </summary>
    public class SyncItem : ISyncItem
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public SyncAction Action { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public ILibraryItem Item { get; set; }

        /// <summary>
        /// Gets or sets the remote item.
        /// </summary>
        /// <value>
        /// The remote item.
        /// </value>
        public ILibraryItem RemoteItem { get; set; }        
    }
}
namespace Surtr.LibraryManagement.Interface
{
    using System;

    /// <summary>
    /// Synchronization interface
    /// </summary>
    public interface ISynchronizeService
    {
        /// <summary>
        /// Occurs when [new synchronize item].
        /// </summary>
        event Action<ISyncItem> NewSyncItem;

        /// <summary>
        /// Synchronizes the specified library.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="remoteFolder">The remote folder.</param>
        /// <param name="options">The options.</param>
        void Synchronize(ILibrary library, string remoteFolder, SynchronizationOptions options);

        /// <summary>
        /// Synchronizes the specified library.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="remoteLibrary">The remote library.</param>
        /// <param name="options">The options.</param>
        void Synchronize(ILibrary library, ILibrary remoteLibrary, SynchronizationOptions options);

        /// <summary>
        /// Sets the synchronize action.
        /// </summary>
        /// <param name="syncItem">The synchronize item.</param>
        /// <param name="options">The options.</param>
        void SetSyncAction(ISyncItem syncItem, SynchronizationOptions options);
    }
}

namespace surtr.LibraryManagement.Interface
{
    using System;

    /// <summary>
    /// Synchronization interface
    /// </summary>
    public interface ISynchronizeService
    {
        event Action<ISyncItem> NewSyncItem;
 
        /// <summary>
        /// Synchronizes a library to another folder
        /// </summary>
        /// <param name="library"></param>
        /// <param name="remoteFolder"></param>
        /// <param name="options"></param>
        void Synchronize(ILibrary library, string remoteFolder, SynchronizationOptions options);

        /// <summary>
        /// Synchronize a library to another library
        /// </summary>
        /// <param name="library"></param>
        /// <param name="remoteLibrary"></param>
        /// <param name="options"></param>
        void Synchronize(ILibrary library, ILibrary remoteLibrary, SynchronizationOptions options);

        /// <summary>
        /// Set the sync action, depending on the options
        /// </summary>
        /// <param name="syncItem"></param>
        /// <param name="options"></param>
        void SetSyncAction(ISyncItem syncItem, SynchronizationOptions options);
    }
}

namespace Surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Synchronization actions enumeration.
    /// </summary>
    public enum SyncAction
    {
        /// <summary>
        /// The no action
        /// </summary>
        Nothing,

        /// <summary>
        /// The copy to remote action
        /// </summary>
        CopyToRemote,

        /// <summary>
        /// The delete from remote action
        /// </summary>
        DeleteFromRemote,

        /// <summary>
        /// The remove from local library action
        /// </summary>
        RemoveFromLocalLibrary,

        /// <summary>
        /// The delete from local action
        /// </summary>
        DeleteFromLocal
    }
}
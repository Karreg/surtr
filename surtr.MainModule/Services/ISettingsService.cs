namespace Surtr.MainModule.Services
{
    /// <summary>
    /// The SettingsService interface.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the local library folder.
        /// </summary>
        /// <value>
        /// The local library folder.
        /// </value>
        string LocalLibraryFolder { get; set; }

        /// <summary>
        /// Gets or sets the remote library folder.
        /// </summary>
        /// <value>
        /// The remote library folder.
        /// </value>
        string RemoteLibraryFolder { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the remote.
        /// </summary>
        /// <value>
        /// The maximum size of the remote.
        /// </value>
        string MaximumRemoteSize { get; set; }
    }
}

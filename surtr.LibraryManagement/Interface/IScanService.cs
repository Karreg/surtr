namespace surtr.LibraryManagement.Interface
{
    using System;
    using Common.Logging.Configuration;

    /// <summary>
    /// Interface for scan service
    /// </summary>
    public interface IScanService
    {
        /// <summary>
        /// Creates a library. Items are added asynchronously.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        ILibrary ScanLibrary(string folder);

        /// <summary>
        /// Update the library asynchronously.
        /// </summary>
        /// <param name="library"></param>
        void UpdateLibrary(ILibrary library);

        /// <summary>
        /// Event to give progress
        /// </summary>
        event Action<string> CurrentDirectory;
    }
}

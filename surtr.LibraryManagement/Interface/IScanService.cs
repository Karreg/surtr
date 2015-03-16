namespace Surtr.LibraryManagement.Interface
{
    using System;

    /// <summary>
    /// Interface for scan service
    /// </summary>
    public interface IScanService
    {
        /// <summary>
        /// Event to give progress
        /// </summary>
        event Action<string> CurrentDirectory;
        
        /// <summary>
        /// Creates a library. Items are added asynchronously.
        /// </summary>
        /// <param name="folder">
        /// The folder containing the library.
        /// </param>
        /// <returns>
        /// The library
        /// </returns>
        ILibrary ScanLibrary(string folder);
    }
}

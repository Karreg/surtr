namespace Surtr.LibraryManagement.Interface
{
    using System;

    /// <summary>
    /// Interface for storing / loading libraries
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// Occurs when [loading].
        /// </summary>
        event Action Loading;

        /// <summary>
        /// Occurs when [loaded].
        /// </summary>
        event Action Loaded;

        /// <summary>
        /// Occurs when [saving].
        /// </summary>
        event Action<double> Saving;

        /// <summary>
        /// Occurs when [saved].
        /// </summary>
        event Action Saved;

        /// <summary>
        /// Stores the library in a file
        /// </summary>
        /// <param name="library">The library</param>
        /// <param name="filename">The filename</param>
        void Store(ILibrary library, string filename);

        /// <summary>
        /// Loads a library from a file, synchronously.
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <returns>The library</returns>
        ILibrary Load(string filename);
    }
}

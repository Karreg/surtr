namespace surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Interface for storing / loading libraries
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// Stores the library in a file
        /// </summary>
        /// <param name="library"></param>
        /// <param name="filename"></param>
        void Store(ILibrary library, string filename);

        /// <summary>
        /// Loads a library from a file, synchronously.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        ILibrary Load(string filename);
    }
}

namespace Surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Interface for type conversion.
    /// </summary>
    public interface IConversionService
    {
        /// <summary>
        /// Formats the extension.
        /// </summary>
        /// <param name="libraryItem">The library item.</param>
        /// <returns>The new library item.</returns>
        ILibraryItem FormatExtension(ILibraryItem libraryItem);

        /// <summary>
        /// Converts to zip.
        /// </summary>
        /// <param name="libraryItem">The library item.</param>
        /// <returns>The new library item.</returns>
        ILibraryItem ConvertToZip(ILibraryItem libraryItem);
    }
}

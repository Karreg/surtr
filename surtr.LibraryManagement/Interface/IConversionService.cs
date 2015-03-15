namespace surtr.LibraryManagement.Interface
{
    /// <summary>
    /// Interface for type conversion.
    /// </summary>
    interface IConversionService
    {
        /// <summary>
        /// Formats the extension.
        /// </summary>
        /// <param name="libraryItem">The library item.</param>
        /// <returns></returns>
        ILibraryItem FormatExtension(ILibraryItem libraryItem);

        /// <summary>
        /// Converts to zip.
        /// </summary>
        /// <param name="libraryItem">The library item.</param>
        /// <returns></returns>
        ILibraryItem ConvertToZip(ILibraryItem libraryItem);
    }
}

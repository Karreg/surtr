namespace surtr.LibraryManagement.Interface
{
    interface IConversionService
    {
        ILibraryItem FormatExtension(ILibraryItem libraryItem);

        ILibraryItem ConvertToZip(ILibraryItem libraryItem);
    }
}

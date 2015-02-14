namespace surtr.LibraryManagement.Interface
{
    using System;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Interface for library items
    /// </summary>
    public interface ILibraryItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Is the item a favorite ? (useful until playlist are handled)
        /// </summary>
        bool Favorite { get; set; }

        /// <summary>
        /// Item name (i.e. filename without extension)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Add date
        /// </summary>
        DateTime AddDate { get; set; }

        /// <summary>
        /// Gets the library path. i.e. the relative path to the library folder.
        /// </summary>
        string LibraryPath { get; }

        /// <summary>
        /// Full path of the item, without the filename
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Filename, with extension.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// Full path filename, with extension.
        /// </summary>
        string FullPathFilename { get; }

        /// <summary>
        /// Last modification date
        /// </summary>
        DateTime LastModificationDate { get; }

        /// <summary>
        /// Creation date
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// True if file exists. False either way.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Size in GB
        /// </summary>
        double Size { get; }
    }
}

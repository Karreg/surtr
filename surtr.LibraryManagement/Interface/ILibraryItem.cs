namespace Surtr.LibraryManagement.Interface
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Interface for library items
    /// </summary>
    public interface ILibraryItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ILibraryItem"/> is favorite.
        /// </summary>
        /// <value>
        ///   <c>true</c> if favorite; otherwise, <c>false</c>.
        /// </value>
        bool Favorite { get; set; }

        /// <summary>
        /// Gets the Item name (i.e. filename without extension)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the Add date
        /// </summary>
        DateTime AddDate { get; set; }

        /// <summary>
        /// Gets the library path. i.e. the relative path to the library folder.
        /// </summary>
        string LibraryPath { get; }

        /// <summary>
        /// Gets the Full path of the item, without the filename
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the Filename, with extension.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// Gets the Full path filename, with extension.
        /// </summary>
        string FullPathFilename { get; }

        /// <summary>
        /// Gets the Last modification date
        /// </summary>
        DateTime LastModificationDate { get; }

        /// <summary>
        /// Gets the Creation date
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ILibraryItem"/> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Gets the Size in GB
        /// </summary>
        double Size { get; }
    }
}

namespace surtr.LibraryManagement.Interface
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for library.
    /// </summary>
    public interface ILibrary
    {
        /// <summary>
        /// Event for new items
        /// </summary>
        event Action<ILibraryItem> NewItem;

        /// <summary>
        /// Event for updated items
        /// </summary>
        event Action<ILibraryItem> UpdatedItem;

        /// <summary>
        /// The root directory of the library
        /// </summary>
        string RootDirectory { get; }

        /// <summary>
        /// List of items in the library
        /// </summary>
        IEnumerable<ILibraryItem> Items { get; }

        /// <summary>
        /// Add the library item to the library
        /// </summary>
        /// <param name="libraryItem"></param>
        void AddItem(ILibraryItem libraryItem);

        /// <summary>
        /// Removes the library item from the library
        /// </summary>
        /// <param name="libraryItemName"></param>
        void RemoveItem(string libraryItemName);

        /// <summary>
        /// Deletes the library item. Physically I mean.
        /// </summary>
        /// <param name="libraryItemName"></param>
        void DeleteItem(string libraryItemName);
    }
}

namespace Surtr.LibraryManagement.Interface
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
        /// Gets the root directory of the library
        /// </summary>
        string RootDirectory { get; }

        /// <summary>
        /// Gets the list of items in the library
        /// </summary>
        IEnumerable<ILibraryItem> Items { get; }

        /// <summary>
        /// Adds the library item to the library
        /// </summary>
        /// <param name="libraryItem">
        /// The library Item.
        /// </param>
        void AddItem(ILibraryItem libraryItem);

        /// <summary>
        /// Removes the library item from the library
        /// </summary>
        /// <param name="libraryItemName">The library Item.</param>
        void RemoveItem(string libraryItemName);

        /// <summary>
        /// Deletes the library item. Physically I mean.
        /// </summary>
        /// <param name="libraryItemName">The library Item.</param>
        void DeleteItem(string libraryItemName);
    }
}

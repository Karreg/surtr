namespace Surtr.LibraryManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Surtr.LibraryManagement.Interface;

    /// <summary>
    /// The library.
    /// </summary>
    public class Library : ILibrary
    {
        /// <summary>
        /// The items.
        /// </summary>
        private readonly IDictionary<string, ILibraryItem> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="Library"/> class.
        /// </summary>
        /// <param name="rootDirectory">
        /// The root directory.
        /// </param>
        public Library(string rootDirectory)
        {
            this.RootDirectory = rootDirectory;
            this.items = new SortedDictionary<string, ILibraryItem>();
        }

        /// <summary>
        /// The new item.
        /// </summary>
        public event Action<ILibraryItem> NewItem;

        /// <summary>
        /// The updated item.
        /// </summary>
        public event Action<ILibraryItem> UpdatedItem;

        /// <summary>
        /// Gets the root directory.
        /// </summary>
        public string RootDirectory { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IEnumerable<ILibraryItem> Items
        {
            get { return this.items.Values; }
        }

        /// <summary>
        /// The add item.
        /// </summary>
        /// <param name="libraryItem">
        /// The library item.
        /// </param>
        public void AddItem(ILibraryItem libraryItem)
        {
            if (!this.items.ContainsKey(libraryItem.Name))
            {
                this.items.Add(libraryItem.Name, libraryItem);
                if (this.NewItem != null)
                {
                    this.NewItem(libraryItem);
                }
            }
            else
            {
                // TODO Check if this makes sense
                this.items[libraryItem.Name] = libraryItem;
                if (this.UpdatedItem != null)
                {
                    this.UpdatedItem(libraryItem);
                }
            }
        }

        /// <summary>
        /// The remove item.
        /// </summary>
        /// <param name="libraryItemName">
        /// The library item name.
        /// </param>
        public void RemoveItem(string libraryItemName)
        {
            if (this.items.ContainsKey(libraryItemName))
            {
                this.items.Remove(libraryItemName);
            }
        }

        /// <summary>
        /// The delete item.
        /// </summary>
        /// <param name="libraryItemName">
        /// The library item name.
        /// </param>
        public void DeleteItem(string libraryItemName)
        {
            ILibraryItem item;
            if (this.items.TryGetValue(libraryItemName, out item))
            {
                if (File.Exists(item.FullPathFilename))
                {
                    File.Delete(item.FullPathFilename);
                }
            }

            this.RemoveItem(libraryItemName);
        }
    }
}
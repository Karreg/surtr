namespace surtr.LibraryManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Interface;

    public class Library : ILibrary
    {
        private readonly IDictionary<string, ILibraryItem> items;

        public Library(string rootDirectory)
        {
            this.RootDirectory = rootDirectory;
            this.items = new SortedDictionary<string, ILibraryItem>();
        }

        public event Action<ILibraryItem> NewItem;
        public event Action<ILibraryItem> UpdatedItem;
        public string RootDirectory { get; private set; }

        public IEnumerable<ILibraryItem> Items
        {
            get { return this.items.Values; }
        }
        
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

        public void RemoveItem(string libraryItemName)
        {
            if (this.items.ContainsKey(libraryItemName))
            {
                this.items.Remove(libraryItemName);
            }
        }

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
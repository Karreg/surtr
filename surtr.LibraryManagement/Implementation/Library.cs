namespace surtr.LibraryManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Interface;

    public class Library : ILibrary
    {
        private readonly IDictionary<string, ILibraryItem> items;

        public Library(string rootDirectory)
        {
            this.RootDirectory = rootDirectory;
            this.items = new Dictionary<string, ILibraryItem>();
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

        public void RemoveItem(ILibraryItem libraryItem)
        {
            if (this.items.ContainsKey(libraryItem.Name))
            {
                this.items.Remove(libraryItem.Name);
            }
        }

        public void DeleteItem(ILibraryItem libraryItem)
        {
            // TODO Check if Path is the correct property to use
            this.RemoveItem(libraryItem);
            if (File.Exists(libraryItem.Path))
            {
                File.Delete(libraryItem.Path);
            }
        }
    }
}
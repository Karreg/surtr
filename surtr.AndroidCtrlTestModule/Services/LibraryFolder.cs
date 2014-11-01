using System.Collections.Generic;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class LibraryFolder
    {
        private readonly Dictionary<string, LibraryItem> items;

        private readonly Dictionary<string, LibraryFolder> subFolders;

        public LibraryFolder(string path)
        {
            this.Path = path;
            this.items = new Dictionary<string, LibraryItem>();
            this.subFolders = new Dictionary<string, LibraryFolder>();
        }

        public string Path { get; private set; }

        public IEnumerable<LibraryFolder> SubFolders { get { return this.subFolders.Values; } }

        public IEnumerable<LibraryItem> Items { get { return this.items.Values; } }

        public void AddFolder(LibraryFolder folder)
        {
            this.subFolders[folder.Path] = folder;
        }

        public void AddItem(LibraryItem libraryItem)
        {
            this.items[libraryItem.Path] = libraryItem;
        }

        public void AddItems(IEnumerable<LibraryItem> libraryItems)
        {
            foreach (var libraryItem in libraryItems)
            {
                this.AddItem(libraryItem);
            }
        }
    }
}

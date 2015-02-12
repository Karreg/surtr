namespace surtr.LibraryManagement.Implementation
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Interface;

    public class ScanService : IScanService
    {
        public ILibrary ScanLibrary(string folder)
        {
            var library = new Library(folder);
            var items = this.ScanFolder(folder, folder);
            foreach (var libraryItem in items)
            {
                library.AddItem(libraryItem);
            }

            return library;
        }

        private IEnumerable<ILibraryItem> ScanFolder(string rootDirectory, string directory)
        {
            var items = new List<ILibraryItem>();
            if (Directory.Exists(directory))
            {
                var files = Directory.EnumerateFiles(directory);
                foreach (var file in files)
                {
                    var libraryPath = directory.Replace(rootDirectory, string.Empty);
                    if (libraryPath.StartsWith(string.Empty + Path.DirectorySeparatorChar))
                    {
                        libraryPath = libraryPath.Substring(1);
                    }

                    var item = new LibraryItem(rootDirectory, libraryPath, Path.GetFileName(file));
                    items.Add(item);
                }

                var directories = Directory.EnumerateDirectories(directory);
                foreach (var subDir in directories)
                {
                    var fullDir = Path.Combine(directory, subDir);
                    items.AddRange(this.ScanFolder(rootDirectory, fullDir));
                }
            }

            return items;
        }

        public void UpdateLibrary(ILibrary library)
        {
            throw new System.NotImplementedException();
        }
    }
}
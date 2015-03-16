namespace Surtr.LibraryManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Surtr.LibraryManagement.Interface;

    /// <summary>
    /// The scan service.
    /// </summary>
    public class ScanService : IScanService
    {
        /// <summary>
        /// The current directory.
        /// </summary>
        public event Action<string> CurrentDirectory;

        /// <summary>
        /// The scan library.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <returns>
        /// The <see cref="ILibrary"/>.
        /// </returns>
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

        /// <summary>
        /// The is a comic.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsBd(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (extension != null)
            {
                extension = extension.ToLowerInvariant();
                return
                    extension.Equals(".cbz") ||
                    extension.Equals(".cbr") ||
                    extension.Equals(".zip") ||
                    extension.Equals(".rar") ||
                    extension.Equals(".pdf");
            }

            return false;
        }

        /// <summary>
        /// The scan folder.
        /// </summary>
        /// <param name="rootDirectory">
        /// The root directory.
        /// </param>
        /// <param name="directory">
        /// The directory.
        /// </param>
        /// <returns>
        /// The enumerable.
        /// </returns>
        private IEnumerable<ILibraryItem> ScanFolder(string rootDirectory, string directory)
        {
            if (this.CurrentDirectory != null)
            {
                this.CurrentDirectory(directory);
            }

            var items = new List<ILibraryItem>();
            if (Directory.Exists(directory))
            {
                var files = Directory.EnumerateFiles(directory);
                foreach (var file in files)
                {
                    if (IsBd(file))
                    {
                        var libraryPath = directory.Replace(rootDirectory, string.Empty);
                        if (libraryPath.StartsWith(string.Empty + Path.DirectorySeparatorChar))
                        {
                            libraryPath = libraryPath.Substring(1);
                        }

                        var item = new LibraryItem(rootDirectory, libraryPath, Path.GetFileName(file));
                        items.Add(item);
                    }
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
    }
}
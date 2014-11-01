using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace surtr.AndroidCtrlTestModule.Services
{
    using System.Globalization;

    interface ILibraryScanner
    {
        LibraryFolder Scan(string rootPath, string subPath);
    }

    public class LibraryScanner : ILibraryScanner
    {
        public Library Scan(string rootPath)
        {
            var libraryFolder = this.Scan(rootPath, string.Empty);
            var library = new Library(rootPath, libraryFolder);
            return library;
        }

        public LibraryFolder Scan(string rootPath, string subPath)
        {
            var currentFolder = Path.Combine(rootPath, subPath);
            currentFolder = currentFolder.Replace(@"\\", @"\");
            var library = new LibraryFolder(subPath);
            if (Directory.Exists(currentFolder))
            {
                var subFolders = Directory.EnumerateDirectories(currentFolder);
                foreach (var subFolderFullPath in subFolders)
                {
                    var subFolder = Path.GetFileName(subFolderFullPath);
                    var subLibrary = this.Scan(rootPath, Path.Combine(subPath, subFolder));
                    library.AddFolder(subLibrary);
                    library.AddItems(subLibrary.Items);
                }

                var items = Directory.EnumerateFiles(currentFolder);
                foreach (var itemFullPath in items)
                {
                    var item = Path.GetFileName(itemFullPath);
                    item = Path.Combine(subPath, item);
                    var libraryItem = new LibraryItem(item);
                    libraryItem.AddDate = DateTime.Now;
                    library.AddItem(libraryItem);
                }
            }

            return library;
        }

        public void Save(Library library, string path)
        {
            var stream = new StreamWriter(path);
            stream.WriteLine(library.RootPath);
            foreach (var libraryItem in library.LibraryFolder.Items.OrderBy(item => item.Path))
            {
                stream.WriteLine("{0};{1};{2}", libraryItem.Path, libraryItem.AddDate.ToString(CultureInfo.InvariantCulture), libraryItem.Favorite);
            }
            stream.Close();
        }

        public Library Load(string path)
        {
            var stream = new StreamReader(path);
            var rootPath = stream.ReadLine();
            var libraryFolder = new LibraryFolder(string.Empty);
            string line;
            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                var values = line.Split(';');
                var item = new LibraryItem(values[0]);
                item.AddDate = DateTime.Parse(values[1], CultureInfo.InvariantCulture);
                item.Favorite = bool.Parse(values[2]);
                libraryFolder.AddItem(item);
            }
            stream.Close();
            var library = new Library(rootPath, libraryFolder);
            return library;
        }
    }

    public class Library
    {
        public Library(string rootPath, LibraryFolder libraryFolder)
        {
            this.RootPath = rootPath;
            this.LibraryFolder = libraryFolder;
        }

        public LibraryFolder LibraryFolder { get; private set; }

        public string RootPath { get; private set; }
    }
}

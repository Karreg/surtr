using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace surtr.AndroidCtrlTestModule.Services
{
    interface ILibraryScanner
    {
        LibraryFolder Scan(string rootPath);
    }

    class LibraryScanner : ILibraryScanner
    {
        public LibraryFolder Scan(string rootPath)
        {
            var library = new LibraryFolder(rootPath);
            if (Directory.Exists(rootPath))
            {
                var subFolders = Directory.EnumerateDirectories(rootPath);
                foreach (var subFolder in subFolders)
                {
                    var subLibrary = this.Scan(Path.Combine(rootPath, subFolder));
                }

                var items = Directory.EnumerateFiles(rootPath);
                foreach (var item in items)
                {
                    var libraryItem = new LibraryItem(Path.Combine(rootPath, item));
                }
            }

            return library;
        }
    }
}

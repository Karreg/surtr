using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace surtr.AndroidCtrlTestModule.Services
{
    class LibraryFolder
    {
        private readonly List<LibraryItem> items;

        private readonly List<LibraryFolder> subFolders;

        public LibraryFolder(string path)
        {
            this.Path = path;
            this.items = new List<LibraryItem>();
            this.subFolders = new List<LibraryFolder>();
        }

        public string Path { get; private set; }

        public IEnumerable<LibraryFolder> SubFolders { get { return this.subFolders; } }

        public IEnumerable<LibraryItem> Items { get { return this.items; } }
    }
}

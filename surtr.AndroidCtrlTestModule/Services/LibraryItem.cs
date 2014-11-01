using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class LibraryItem
    {
        public LibraryItem(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        public bool Favorite { get; set; }

        public DateTime AddDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace surtr.Database
{
    /// <summary>
    /// POCO Model for database
    /// </summary>
    public class LibraryPoco
    {
        public string RootPath { get; set; }

        public string Name { get; set; }

        public List<string> BookIds { get; set; }
    }
}

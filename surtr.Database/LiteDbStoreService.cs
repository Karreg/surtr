namespace surtr.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using LiteDB;

    using Surtr.LibraryManagement.Implementation;
    using Surtr.LibraryManagement.Interface;

    public class LiteDbStoreService : IStoreService
    {
        public event Action Loading;

        public event Action Loaded;

        public event Action<double> Saving;

        public event Action Saved;

        public void Store(ILibrary library, string filename)
        {
            using (var db = new LiteDatabase(filename))
            {
                var libraries = db.GetCollection<LibraryPoco>("Libraries");
                LibraryPoco libraryPoco;
                if (libraries.Exists(l => l.Name == "Default"))
                {
                    libraryPoco = libraries.FindOne(l => l.Name == "Default");
                }
                else
                {
                    libraryPoco = new LibraryPoco { Name = "Default", RootPath = library.RootDirectory };
                    libraryPoco.BookIds = new List<string>();
                }

                foreach (var libraryItem in library.Items)
                {
                    // TODO Get or update
                }
            }
        }

        public ILibrary Load(string filename)
        {
            using (var db = new LiteDatabase(filename))
            {
                var libraryPoco = db.GetCollection<LibraryPoco>("Libraries").FindAll().FirstOrDefault();
                if (libraryPoco != null)
                {
                    var library = new Library(libraryPoco.RootPath);
                    var booksPoco = db.GetCollection<BookPoco>("Books");
                    foreach (var bookId in libraryPoco.BookIds)
                    {
                        var bookPoco = booksPoco.FindOne(b => b.BookPathInLibrary.Equals(bookId));
                        if (bookPoco != null)
                        {
                            var bookFolder = Path.GetDirectoryName(bookPoco.BookPathInLibrary);
                            var bookFilename = Path.GetFileName(bookPoco.BookPathInLibrary);
                            LibraryItem libraryItem = new LibraryItem(libraryPoco.RootPath, bookFolder, bookFilename);
                            library.AddItem(libraryItem);
                        }
                    }
                    return library;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
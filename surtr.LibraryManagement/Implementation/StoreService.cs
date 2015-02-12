namespace surtr.LibraryManagement.Implementation
{
    using System;
    using System.IO;
    using Interface;

    public class StoreService : IStoreService
    {
        public void Store(ILibrary library, string filename)
        {
            var output = new StreamWriter(filename);

            foreach (var libraryItem in library.Items)
            {
                output.WriteLine("{0};{1};{2};{3}", libraryItem.LibraryPath, libraryItem.Filename, libraryItem.Favorite, libraryItem.AddDate);
            }

            output.Close();
        }

        public ILibrary Load(string filename)
        {
            var rootDirectory = Path.GetDirectoryName(filename);
            var library = new Library(rootDirectory);
            var input = File.OpenText(filename);

            string line;
            while ((line = input.ReadLine()) != null)
            {
                var values = line.Split(';');
                if (values.Length == 4)
                {
                    var item = new LibraryItem(rootDirectory, values[0], values[1]);
                    item.Favorite = bool.Parse(values[2]);
                    item.AddDate = DateTime.Parse(values[3]);
                    library.AddItem(item);
                }
            }

            input.Close();

            return library;
        }
    }
}
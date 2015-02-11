namespace surtr.LibraryManagement.Implementation
{
    using System.IO;
    using Interface;

    public class StoreService : IStoreService
    {
        public void Store(ILibrary library, string filename)
        {
            var output = new StreamWriter(filename);

            foreach (var libraryItem in library.Items)
            {
                output.WriteLine("{0};{1};{2}", libraryItem.LibraryPath, libraryItem.Name, libraryItem.Favorite);
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
                if (values.Length == 3)
                {
                    var item = new LibraryItem(Path.Combine(rootDirectory, values[0]), values[1]);
                    item.Favorite = bool.Parse(values[2]);
                    library.AddItem(item);
                }
            }

            input.Close();

            return library;
        }
    }
}
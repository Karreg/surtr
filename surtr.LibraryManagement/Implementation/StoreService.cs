namespace surtr.LibraryManagement.Implementation
{
    using System;
    using System.IO;
    using System.Linq;
    using Interface;

    public class StoreService : IStoreService
    {
        private readonly IScanService scanService;

        public StoreService(IScanService scanService)
        {
            this.scanService = scanService;
        }

        public void Store(ILibrary library, string filename)
        {
            if (Directory.Exists(Path.GetDirectoryName(filename)))
            {
                var output = new StreamWriter(filename);

                foreach (var libraryItem in library.Items)
                {
                    output.WriteLine("{0};{1};{2};{3}", libraryItem.LibraryPath, libraryItem.Filename, libraryItem.Favorite, libraryItem.AddDate);
                }

                output.Close();
            }
        }

        public ILibrary Load(string filename)
        {
            var rootDirectory = Path.GetDirectoryName(filename);

            var library = this.scanService.ScanLibrary(rootDirectory);

            var input = File.OpenText(filename);

            string line;
            while ((line = input.ReadLine()) != null)
            {
                var values = line.Split(';');
                if (values.Length == 4)
                {
                    
                    var item = new LibraryItem(rootDirectory, values[0], values[1]);
                    var existingItem = library.Items.FirstOrDefault(i => i.Name.Equals(item.Name));
                    if (existingItem != null)
                    {
                        existingItem.Favorite = bool.Parse(values[2]);
                        existingItem.AddDate = DateTime.Parse(values[3]);
                    }
                    else
                    {
                        item.Favorite = bool.Parse(values[2]);
                        item.AddDate = DateTime.Parse(values[3]);
                        library.AddItem(item);
                    }
                }
            }

            input.Close();

            return library;
        }
    }
}
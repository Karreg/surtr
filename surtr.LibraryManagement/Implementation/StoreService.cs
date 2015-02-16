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

        public event Action Loading;
        public event Action Loaded;
        public event Action<double> Saving;
        public event Action Saved;

        public void Store(ILibrary library, string filename)
        {
            if (Directory.Exists(Path.GetDirectoryName(filename)))
            {
                var output = new StreamWriter(filename);
                var count = library.Items.Count();
                double cpt = 0;
                foreach (var libraryItem in library.Items)
                {
                    cpt++;
                    output.WriteLine("{0};{1};{2};{3}", libraryItem.LibraryPath, libraryItem.Filename, libraryItem.Favorite, libraryItem.AddDate);
                    if (this.Saving != null)
                    {
                        this.Saving(cpt/count);
                    }
                }

                output.Close();

                if (this.Saved != null)
                {
                    this.Saved();
                }
            }
        }

        public ILibrary Load(string filename)
        {
            var rootDirectory = Path.GetDirectoryName(filename);

            var library = this.scanService.ScanLibrary(rootDirectory);

            if (this.Loading != null)
            {
                this.Loading();
            }

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

            if (this.Loaded != null)
            {
                this.Loaded();
            }

            return library;
        }
    }
}
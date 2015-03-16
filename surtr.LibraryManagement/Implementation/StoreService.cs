namespace Surtr.LibraryManagement.Implementation
{
    using System;
    using System.IO;
    using System.Linq;
    using Interface;

    /// <summary>
    /// The store service.
    /// </summary>
    public class StoreService : IStoreService
    {
        /// <summary>
        /// The scan service
        /// </summary>
        private readonly IScanService scanService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreService"/> class.
        /// </summary>
        /// <param name="scanService">The scan service.</param>
        public StoreService(IScanService scanService)
        {
            this.scanService = scanService;
        }

        /// <summary>
        /// Occurs when [loading].
        /// </summary>
        public event Action Loading;
        
        /// <summary>
        /// Occurs when [loaded].
        /// </summary>
        public event Action Loaded;
        
        /// <summary>
        /// Occurs when [saving].
        /// </summary>
        public event Action<double> Saving;

        /// <summary>
        /// Occurs when [saved].
        /// </summary>
        public event Action Saved;

        /// <summary>
        /// Stores the library in a file
        /// </summary>
        /// <param name="library">The library</param>
        /// <param name="filename">The filename</param>
        public void Store(ILibrary library, string filename)
        {
            var directory = Path.GetDirectoryName(filename);
            if (directory != null && Directory.Exists(directory))
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
                        this.Saving(cpt / count);
                    }
                }

                output.Close();

                if (this.Saved != null)
                {
                    this.Saved();
                }
            }
        }

        /// <summary>
        /// Loads a library from a file, synchronously.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The loaded library.</returns>
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
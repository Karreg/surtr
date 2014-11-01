namespace surtr.Spikes
{
    using System.IO;
    using AndroidCtrlTestModule.Services;

    class LibraryScanning
    {
        private readonly string folder;
        private readonly string file;
        private readonly string file2;

        public LibraryScanning()
        {
            this.folder = @"C:\Users\kryst_000\OneDrive";
            this.file = @"C:\Users\kryst_000\Workspaces\surtr\library.txt";
            this.file2 = @"C:\Users\kryst_000\Workspaces\surtr\library2.txt";
        }

        public void ScanAndSave()
        {
            var scanner = new LibraryScanner();

            //if (!File.Exists(this.file))
            {
                var library = scanner.Scan(this.folder);
                scanner.Save(library, this.file);
                System.Diagnostics.Process.Start(this.file);
            }

            var loadedLibrary = scanner.Load(this.file);
            scanner.Save(loadedLibrary, this.file2);
            // System.Diagnostics.Process.Start(this.file2);
        }
    }
}

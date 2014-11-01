using System;
using System.ComponentModel;

namespace surtr.Spikes
{
    class Program
    {
        public static void Main(string[] args)
        {
            // TestAndroidConnectivity();
            TestLibraryScanning();
        }

        private static void TestLibraryScanning()
        {
            var libraryScanning = new LibraryScanning();
            libraryScanning.ScanAndSave();
        }

        private static void TestAndroidConnectivity()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, eventArgs) =>
            {
                var connectivity = new AndroidConnectivity();
                connectivity.Connect();
                connectivity.Start();
            };

            worker.RunWorkerAsync();

            Console.ReadLine();

            worker.Dispose();
        }
    }
}

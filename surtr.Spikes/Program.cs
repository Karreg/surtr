using System;
using System.ComponentModel;

namespace surtr.Spikes
{
    class Program
    {
        public static void Main(string[] args)
        {
            AndroidConnectivity connectivity;

            var worker = new BackgroundWorker();
            worker.DoWork += (sender, eventArgs) =>
            {
                connectivity = new AndroidConnectivity();
                connectivity.Connect();
                connectivity.Start();
            };

            worker.RunWorkerAsync();

            Console.ReadLine();

            worker.Dispose();
        }
    }
}

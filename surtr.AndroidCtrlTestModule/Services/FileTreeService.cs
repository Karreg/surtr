using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AndroidCtrl.ADB;
using Michonne.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class FileTreeService : BindableBase, IDisposable
    {
        private const string DefaultFolder = "/sdcard/eBooks";
        private readonly IUnitOfExecution dispatcher;

        public void SetDevice(Device device)
        {
            if (this.FolderSelected != null)
            {
                this.FolderSelected(DefaultFolder);
            }
        }

        public FileTreeService(IUnitOfExecution dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public event Action<string> FolderSelected;

        public event Action<string> FileScanned;

        public event Action<string> DirectoryScanned;

        public void ScanDirectory(string path)
        {
            this.dispatcher.Dispatch(() =>
            {
                var directories = ADB.Instance().Device.Directories(path).GetDirectories();
                if (this.DirectoryScanned != null)
                {
                    foreach (var directory in directories)
                    {
                        this.DirectoryScanned(directory.Name);
                    }
                }

                var files = ADB.Instance().Device.Directories(DefaultFolder).GetFiles();
                if (this.FileScanned != null)
                {
                    foreach (var file in files)
                    {
                        this.FileScanned(file.Name);
                    }
                }
            });
        }

        public void Dispose()
        {
        }
    }
}

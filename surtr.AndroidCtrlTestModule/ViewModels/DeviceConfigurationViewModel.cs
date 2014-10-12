using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AndroidCtrl.ADB;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;

namespace surtr.AndroidCtrlTestModule.ViewModels
{
    public class DeviceConfigurationViewModel : BindableBase
    {
        private string currentFolder;
        private int folderItemCount;
        private readonly FileTreeService fileTreeService;
        private string selectedSubFolder;

        public DeviceConfigurationViewModel(FileTreeService fileTreeService)
        {
            this.FolderItems = new ObservableCollection<string>();
            this.FileItems = new ObservableCollection<string>();

            this.SynchronizeCommand = new DelegateCommand(() => MessageBox.Show("Sync!"));

            this.fileTreeService = fileTreeService;
            this.fileTreeService.FolderSelected += f => this.CurrentFolder = f;
            this.fileTreeService.DirectoryScanned += this.OnDirectoryScanned;
            this.fileTreeService.FileScanned += this.OnFileScanned;
        }

        public string CurrentFolder
        {
            get { return currentFolder; }
            set
            {
                if (value != null && this.currentFolder != value)
                {
                    currentFolder = value;
                    this.OnPropertyChanged("CurrentFolder");
                    this.Rescan();
                }

            }
        }

        public string SelectedSubFolder
        {
            get { return selectedSubFolder; }
            set
            {
                if (value != null)
                {
                    this.CurrentFolder = string.Format("{0}/{1}", this.CurrentFolder, value).Replace("//", "/");
                    this.selectedSubFolder = value;
                }
            }
        }

        private void Rescan()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.FolderItemCount = 0;
                this.FileItems.Clear();
                this.FolderItems.Clear();
            });
            this.fileTreeService.ScanDirectory(this.currentFolder);
        }

        public int FolderItemCount
        {
            get { return folderItemCount; }
            set
            {
                folderItemCount = value;
                this.OnPropertyChanged("FolderItemCount");
            }
        }

        public ObservableCollection<string> FolderItems { get; private set; }

        public ObservableCollection<string> FileItems { get; private set; }

        public ICommand SynchronizeCommand { get; private set; }

        private void OnFileScanned(string file)
        {
            this.FolderItemCount++;
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.FileItems.Add(file);
            });
        }

        private void OnDirectoryScanned(string directory)
        {
            this.FolderItemCount++;
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.FolderItems.Add(directory);
            });
        }
    }
}
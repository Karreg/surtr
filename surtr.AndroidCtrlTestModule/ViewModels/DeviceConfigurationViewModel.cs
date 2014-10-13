using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Michonne.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;

namespace surtr.AndroidCtrlTestModule.ViewModels
{
    public class DeviceConfigurationViewModel : BindableBase
    {
        private List<string> path;
        private string currentFolder;
        private int folderItemCount;
        private readonly IFileTreeService fileTreeService;
        private string selectedSubFolder;
        private IUnitOfExecution dispatcher;
        private const string UpFolder = "..";

        public DeviceConfigurationViewModel(IFileTreeService fileTreeService, IUnitOfExecution dispatcher)
        {
            this.dispatcher = dispatcher;

            this.FolderItems = new ObservableCollection<string>();
            this.FolderItems.Add(UpFolder);
            this.FileItems = new ObservableCollection<string>();
            path = new List<string>();

            this.SynchronizeCommand = new DelegateCommand(() => MessageBox.Show("Sync!"));
            this.ChangeDirectoryCommand = new DelegateCommand(this.Rescan);

            this.fileTreeService = fileTreeService;
            this.fileTreeService.FolderSelected += f =>
            {
                this.CurrentFolder = f;
                this.Rescan();
            };
            this.fileTreeService.DirectoryScanned += this.OnDirectoryScanned;
            this.fileTreeService.FileScanned += this.OnFileScanned;
        }

        public string CurrentFolder
        {
            get { return currentFolder; }
            set
            {
                if (value != null && (this.currentFolder != value || value == UpFolder))
                {
                    currentFolder = value;
                    this.OnPropertyChanged("CurrentFolder");
                    //this.Rescan();
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
                    if (value == UpFolder)
                    {
                        this.path.RemoveAt(this.path.Count-1);
                    }
                    else
                    {
                        this.path.Add(value);
                    }
                    this.CurrentFolder = string.Format("/{0}", this.path.Count > 0 ? this.path.Aggregate((i,j) => i+"/"+j) : string.Empty);
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
                this.FolderItems.Add(UpFolder);
            });
            this.dispatcher.Dispatch(() =>
            {
                this.fileTreeService.ScanDirectory(this.currentFolder);
            });
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

        public ICommand ChangeDirectoryCommand { get; private set; }

        public ICommand SynchronizeCommand { get; private set; }

        private void OnFileScanned(string file)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.FolderItemCount++;
                this.FileItems.Add(file);
            });
        }

        private void OnDirectoryScanned(string directory)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.FolderItemCount++;
                this.FolderItems.Add(directory);
            });
        }
    }
}
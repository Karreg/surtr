namespace surtr.LibraryManagement.Implementation
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Interface;

    public class LibraryItem : ILibraryItem, INotifyPropertyChanged
    {
        private bool favorite;

        public LibraryItem(string rootPath, string libraryPath, string filename)
        {
            this.FullPathFilename = System.IO.Path.Combine(rootPath, libraryPath, filename);
            this.Path = System.IO.Path.GetDirectoryName(this.FullPathFilename);
            this.Filename = System.IO.Path.GetFileName(this.FullPathFilename);
            this.Name = System.IO.Path.Combine(libraryPath, filename);
            this.LibraryPath = libraryPath;
            this.AddDate = DateTime.Now;
        }

        public bool Favorite
        {
            get { return this.favorite; }
            set
            {
                this.favorite = value;
                this.OnPropertyChanged("Favorite");
            }
        }

        public string Name { get; private set; }
        public DateTime AddDate { get; set; }
        public string LibraryPath { get; private set; }
        public string Path { get; private set; }
        public string Filename { get; private set; }
        public string FullPathFilename { get; private set; }
        
        public DateTime LastModificationDate
        {
            get { return File.GetLastWriteTime(this.FullPathFilename); }
        }
        public DateTime CreationDate {
            get { return File.GetCreationTime(this.FullPathFilename); }
        }

        public bool Exists { get { return File.Exists(this.FullPathFilename); } }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
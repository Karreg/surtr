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
        private readonly string fullPathFilename;

        public LibraryItem(string libraryPath, string filename)
        {
            this.fullPathFilename = System.IO.Path.GetFullPath(filename);
            this.Path = System.IO.Path.GetDirectoryName(this.fullPathFilename);
            this.Filename = System.IO.Path.GetFileName(this.fullPathFilename);
            this.Name = System.IO.Path.GetFileNameWithoutExtension(this.fullPathFilename);
            this.LibraryPath = libraryPath;
        }

        public bool Favorite { get; set; }
        public string LibraryPath { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Filename { get; private set; }
        
        public DateTime LastModificationDate
        {
            get { return File.GetLastWriteTime(this.fullPathFilename); }
        }
        public DateTime CreationDate {
            get { return File.GetCreationTime(this.fullPathFilename); }
        }
        public bool Exists { get { return File.Exists(this.fullPathFilename); } }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
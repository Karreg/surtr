namespace surtr.LibraryManagement.Interface
{
    using System;
    using System.IO;

    public class LibraryItem : ILibraryItem
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

        public string Path { get; private set; }
        public string Filename { get; private set; }
        public string Name { get; private set; }
        public string LibraryPath { get; private set; }

        public DateTime LastModificationDate
        {
            get { return File.GetLastWriteTime(this.fullPathFilename); }
        }
        public DateTime CreationDate {
            get { return File.GetCreationTime(this.fullPathFilename); }
        }
        public bool Favorite { get; set; }
        public bool Exists { get { return File.Exists(this.fullPathFilename); } }
    }
}
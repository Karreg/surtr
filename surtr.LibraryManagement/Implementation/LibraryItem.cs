namespace Surtr.LibraryManagement.Implementation
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using Annotations;
    using Interface;

    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The library item.
    /// </summary>
    public class LibraryItem : ILibraryItem
    {
        /// <summary>
        /// The favorite.
        /// </summary>
        private bool favorite;

        private ImageSource image;

        private bool hasCover = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryItem"/> class.
        /// </summary>
        /// <param name="rootPath">
        /// The root path.
        /// </param>
        /// <param name="libraryPath">
        /// The library path.
        /// </param>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public LibraryItem(string rootPath, string libraryPath, string filename)
        {
            this.FullPathFilename = System.IO.Path.Combine(rootPath, libraryPath, filename);
            this.Path = System.IO.Path.GetDirectoryName(this.FullPathFilename);
            this.Filename = System.IO.Path.GetFileName(this.FullPathFilename);
            this.Name = System.IO.Path.Combine(libraryPath, filename);
            this.LibraryPath = libraryPath;
            this.AddDate = DateTime.Now;
            if (this.Exists)
            {
                var fileInfo = new FileInfo(this.FullPathFilename);
                this.Size = ((double)fileInfo.Length) / (1024 * 1024);
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
            }

            this.HasCover = File.Exists(this.FullPathFilename + ".jpg");
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether favorite.
        /// </summary>
        public bool Favorite
        {
            get
            {
                return this.favorite;
            }

            set
            {
                if (this.favorite != value)
                {
                    this.favorite = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the add date.
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// Gets the library path.
        /// </summary>
        public string LibraryPath { get; private set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the full path filename.
        /// </summary>
        public string FullPathFilename { get; private set; }

        /// <summary>
        /// Gets the last modification date.
        /// </summary>
        public DateTime LastModificationDate
        {
            get { return File.GetLastWriteTime(this.FullPathFilename); }
        }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        public DateTime CreationDate
        {
            get { return File.GetCreationTime(this.FullPathFilename); }
        }

        /// <summary>
        /// Gets a value indicating whether exists.
        /// </summary>
        public bool Exists
        {
            get { return File.Exists(this.FullPathFilename); }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public double Size { get; private set; }

        public bool HasCover
        {
            get
            {
                return this.hasCover;
            }

            set
            {
                this.hasCover = value;
                this.OnPropertyChanged("HasCover");
            }
        }

        /// <summary>
        /// Cover for the item
        /// </summary>
        public ImageSource Cover
        {
            get
            {
                if (this.HasCover && this.image == null)
                {
                    this.image = new BitmapImage(new Uri(this.FullPathFilename + ".jpg"));
                }

                return this.image;
            }
        }

        public Visibility CoverVisibility
        {
            get
            {
                if (this.HasCover)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
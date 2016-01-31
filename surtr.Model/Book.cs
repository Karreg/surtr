namespace surtr.Model
{
    using System;
    using System.Drawing;
    using System.IO;

    public class Book
    {
        /// <summary>
        /// Title of the book
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Cover of the book
        /// </summary>
        public Image Cover { get; set; }

        /// <summary>
        /// Date the file was added to database
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// Modification date of the file
        /// </summary>
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// Creation date of the file
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Filepath (without filename)
        /// </summary>
        public string Filepath { get; set; }

        /// <summary>
        /// Full file path
        /// </summary>
        public string FullFilePath => Path.Combine(this.Filepath, this.Filename);

        /// <summary>
        /// ID, using the full file path
        /// </summary>
        public string Id => this.FullFilePath;

        /// <summary>
        /// Size of the file in bytes
        /// </summary>
        public double FileSize { get; set; }
    }
}

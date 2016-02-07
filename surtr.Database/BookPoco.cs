namespace surtr.Database
{
    using System;

    /// <summary>
    /// POCO Model for database
    /// </summary>
    public class BookPoco
    {
        public string BookPathInLibrary { get; set; }

        public string Name { get; set; }

        public byte[] Cover { get; set; }

        public DateTime AddDate { get; set; }
    }
}

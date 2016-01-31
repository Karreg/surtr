namespace surtr.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Definition of a bookset (full library or playlist)
    /// </summary>
    public class BookSet : Dictionary<string, Book>
    {
        public string Name { get; set; }

        public void Add(Book book)
        {
            base.Add(book.Id, book);
        }
    }
}

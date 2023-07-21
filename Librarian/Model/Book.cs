namespace Librarian.Model
{
    public class Book
    {
        public Book(string name, string author)
        {
            Random rnd = new Random();

            bookName = name;
            bookId = rnd.Next(20);
            bookAuthor = author;
        }
        public int bookId { get; }
        public string bookName { get; }
        public string bookAuthor { get; }
    }
}
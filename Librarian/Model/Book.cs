using System.Text.Json.Serialization;

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

        [JsonPropertyName("name")]
        public string bookName { get; }

        [JsonPropertyName("author")]
        public string bookAuthor { get; }
    }
}
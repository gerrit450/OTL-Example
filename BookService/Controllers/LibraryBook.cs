using System.Text.Json.Serialization;

namespace BookService.Controllers
{
    public class LibraryBook
    {
        public LibraryBook()
        {
            Name = string.Empty;
            Author = string.Empty;
        }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

    }
}
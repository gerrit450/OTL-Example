using System.Text.Json.Serialization;

namespace BookStore.Controllers
{
    public class BookServiceBook
    {
        public BookServiceBook()
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
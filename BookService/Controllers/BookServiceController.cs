using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace BookService.Controllers
{
    public class BookServiceController
    {
        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<List<LibraryBook>> GetListOfBooksFromLibrarian()
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("Get all books"))
            {
                var client = new HttpClient();
                var response = await client.GetStringAsync($"https://localhost:1003/Books");
                var libraryBooks = JsonSerializer.Deserialize<List<LibraryBook>>(response) ?? new List<LibraryBook>();

                span?.SetTag("number of books", libraryBooks.Count);

                return libraryBooks;
            }
        }

        [HttpGet]
        [Route("GetBook/{name}")]
        public async Task<LibraryBook> GetBookFromLibrarian(string name)
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("Get book by name"))
            {
                var client = new HttpClient();
                var responseString = await client.GetStringAsync($"https://localhost:1003/Book/{name}");
                var libraryBook = JsonSerializer.Deserialize<LibraryBook>(responseString) ?? new LibraryBook();

                span?.AddTag("library-book.name", libraryBook.Name);
                span?.SetTag("library-book.author", libraryBook.Author);

                return libraryBook;
            }
        }

        [HttpDelete]
        [Route("DeleteBook/{name}")]
        public async Task<string> AskLibrarianToRemoveBook(string name)
        {
            using (var span = DiagnosticsConfig.ActivitySource.StartActivity("Delete book"))
            {
                var client = new HttpClient();
                var response = await client.DeleteAsync($"https://localhost:1003/Book/{name}");

                return response.ToString();
            }
        }
    }
}